#if UNITY_EDITOR
// fix4a capture helpers (editor only)
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AmongUsGameTools
{
	public static class Fix4aCapture
	{
		static readonly BindingFlags F = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
		public static bool Run(string action)
		{
			if (!action.StartsWith("f4a")) return false;
			string arg = action.Contains("=") ? action.Substring(action.IndexOf('=') + 1) : "";
			string name = action.Contains("=") ? action.Substring(0, action.IndexOf('=')) : action;
			try
			{
				switch (name)
				{
					case "f4aui": // UGUI hit test + click through the EventSystem on the active Button named arg
					{
						var es = EventSystem.current;
						Debug.Log("[F4A] eventSystem=" + (es != null) + " module=" + (es != null && es.currentInputModule != null ? es.currentInputModule.GetType().Name : "none"));
						var btn = UnityEngine.Object.FindObjectsByType<Button>(FindObjectsSortMode.None).FirstOrDefault(b => b.name == arg && b.gameObject.activeInHierarchy);
						if (btn == null) { Debug.Log("[F4A] no active button " + arg + " all=" + string.Join(",", UnityEngine.Object.FindObjectsByType<Button>(FindObjectsSortMode.None).Where(b => b.gameObject.activeInHierarchy).Select(b => b.name).ToArray())); break; }
						var cv = btn.GetComponentInParent<Canvas>();
						Vector2 sp = RectTransformUtility.WorldToScreenPoint(cv != null && cv.renderMode != RenderMode.ScreenSpaceOverlay ? cv.worldCamera : null, btn.transform.position);
						var ped = new PointerEventData(es) { position = sp, button = PointerEventData.InputButton.Left };
						var hits = new System.Collections.Generic.List<RaycastResult>();
						es.RaycastAll(ped, hits);
						Debug.Log("[F4A] click " + arg + " at " + sp + " hits=" + string.Join(",", hits.Take(4).Select(h => h.gameObject.name).ToArray()));
						if (hits.Count > 0)
						{
							ped.pointerCurrentRaycast = hits[0]; ped.pointerPressRaycast = hits[0];
							var go = hits[0].gameObject;
							var h = ExecuteEvents.ExecuteHierarchy(go, ped, ExecuteEvents.pointerClickHandler);
							Debug.Log("[F4A] pointerClick handled by " + (h != null ? h.name : "none"));
						}
						break;
					}
					case "f4abtns": // task adder role/task buttons
					{
						var me = PlayerControl.LocalPlayer.Data.Role;
						Debug.Log("[F4A] localRole=" + me.Role + " type=" + me.GetType().Name);
						foreach (var b in UnityEngine.Object.FindObjectsByType<TaskAddButton>(FindObjectsSortMode.None))
							if (b.role) Debug.Log("[F4A] btn " + b.role.Role + " overlayEnabled=" + b.Overlay.enabled + " sprite=" + (b.Overlay.sprite ? b.Overlay.sprite.name : "null") + " pos=" + b.transform.localPosition.ToString("F1") + " text=" + b.Text.text);
						break;
					}
					case "f4afolder":
					{
						var g = UnityEngine.Object.FindFirstObjectByType<TaskAdderGame>();
						var last = g.Hierarchy.Last();
						var sub = last.SubFolders.FirstOrDefault(s => s.FolderName == arg);
						Debug.Log("[F4A] folder " + arg + " found=" + (sub != null) + " options=" + string.Join(",", last.SubFolders.Select(s => s.FolderName).ToArray()));
						if (sub) g.ShowFolder(sub);
						break;
					}
					case "f4apick":
					{
						var b = UnityEngine.Object.FindObjectsByType<TaskAddButton>(FindObjectsSortMode.None).FirstOrDefault(x => x.role && x.role.Role.ToString() == arg);
						Debug.Log("[F4A] pick " + arg + " found=" + (b != null));
						if (b) b.AddTask();
						break;
					}
					case "f4arole":
						PlayerControl.LocalPlayer.RpcSetRole((AmongUs.GameOptions.RoleTypes)Enum.Parse(typeof(AmongUs.GameOptions.RoleTypes), arg, true));
						break;
					case "f4amap":
					{
						var mb = MapBehaviour.Instance;
						Debug.Log("[F4A] MAP inst=" + (mb != null) + " active=" + (mb != null && mb.gameObject.activeSelf) + " pos=" + (mb != null ? mb.transform.position.ToString("F1") : "") + " parent=" + (mb != null && mb.transform.parent ? mb.transform.parent.name : ""));
						if (mb != null) foreach (var sr in mb.GetComponentsInChildren<SpriteRenderer>(false).Take(8)) Debug.Log("[F4A] MAPSR " + sr.name + " en=" + sr.enabled + " vis=" + sr.isVisible + " sprite=" + (sr.sprite ? sr.sprite.name : "null") + " bounds=" + sr.bounds.center.ToString("F1") + "/" + sr.bounds.size.ToString("F1") + " order=" + sr.sortingOrder + " layer=" + LayerMask.LayerToName(sr.gameObject.layer) + " mat=" + (sr.sharedMaterial ? sr.sharedMaterial.shader.name : "null"));
						break;
					}
					default: return false;
				}
			}
			catch (Exception e) { Debug.Log("[F4A] action " + action + " failed: " + e); }
			return true;
		}
	}
}
#endif
