#if UNITY_EDITOR
// fix4b capture helpers (editor only): t4list = list task prefabs of the current map; t4all[=skip:count] = tour them with Tasks3Capture.Tour
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmongUsGameTools
{
	public static class Fix4bCapture
	{
		public static bool Run(string action)
		{
			if (action.StartsWith("t4opts="))
			{
				var o = UnityEngine.Object.FindFirstObjectByType<OptionsMenuBehaviour>(FindObjectsInactive.Include);
				int idx = int.Parse(action.Substring(7));
				if (!o.IsOpen) o.Open();
				o.OpenTabGroup(idx);
				Debug.Log("[F4] opts tabs=" + o.Tabs.Length + " open=" + idx);
				for (int i = 0; i < o.Tabs.Length; i++) Debug.Log("[F4] opts tab" + i + " content=" + (o.Tabs[i].Content ? o.Tabs[i].Content.name + " active=" + o.Tabs[i].Content.activeSelf : "null") + " button=" + (o.Tabs[i].Button ? o.Tabs[i].Button.name : "null"));
				var content = o.Tabs[idx].Content;
				if (content) foreach (var t in content.GetComponentsInChildren<Transform>(false)) { var tmp = t.GetComponent<TMPro.TMP_Text>(); var pb = t.GetComponent<PassiveButton>(); Debug.Log("[F4] opts item " + t.name + " pos=" + t.position.ToString("F2") + (tmp ? " text='" + tmp.text + "'" : "") + (pb ? " PB" : "") + (t.GetComponent<ResolutionSlider>() ? " RES" : "")); }
				return true;
			}
			if (action.StartsWith("t4click="))
			{
				var o = UnityEngine.Object.FindFirstObjectByType<OptionsMenuBehaviour>(FindObjectsInactive.Include);
				string want = action.Substring(8);
				foreach (var pb in o.GetComponentsInChildren<PassiveButton>(false))
					if (pb.name == want)
					{
						Debug.Log("[F4] click " + want + " listeners=" + pb.OnClick.GetPersistentEventCount());
						pb.ReceiveMouseOver(); pb.ReceiveClickDown(); pb.ReceiveClickUp();
						return true;
					}
				Debug.Log("[F4] click not found " + want);
				return true;
			}
			if (action == "t4list" || action.StartsWith("t4all"))
			{
				var ss = ShipStatus.Instance;
				var all = ss.CommonTasks.Concat(ss.LongTasks).Concat(ss.ShortTasks).Where(t => t).ToList();
				foreach (var t in all)
				{
					int cons = UnityEngine.Object.FindObjectsByType<Console>(FindObjectsSortMode.None).Count(c => c.TaskTypes != null && c.TaskTypes.Contains(t.TaskType));
					Debug.Log("[F4] task " + t.TaskType + " mg=" + (t.MinigamePrefab ? t.MinigamePrefab.GetType().Name + "/" + t.MinigamePrefab.name : "null") + " steps=" + t.MaxStep + " consoles=" + cons + " cls=" + t.GetType().Name);
				}
				if (action.StartsWith("t4all"))
				{
					int skip = 0, count = 999;
					if (action.Contains("=")) { var p = action.Split('=')[1].Split(':'); skip = int.Parse(p[0]); count = int.Parse(p[1]); }
					var names = all.Select(t => t.TaskType.ToString()).Distinct().Skip(skip).Take(count).ToArray();
					var m = typeof(Tasks3Capture).GetMethod("Tour", BindingFlags.Static | BindingFlags.NonPublic);
					HudManager.Instance.StartCoroutine((IEnumerator)m.Invoke(null, new object[] { names }));
				}
				return true;
			}
			return false;
		}
	}
}
#endif
