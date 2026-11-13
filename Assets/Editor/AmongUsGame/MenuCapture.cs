#if UNITY_EDITOR
// Graphics-enabled capture driver (run WITHOUT -nographics). Usage:
//   Unity -batchmode -projectPath AmongUsGame -executeMethod AmongUsGameTools.MenuCapture.Start -logFile x.log -- scene=MainMenu out=C:/x.png frames=120 w=1920 h=1080
// Enters play mode, waits N frames, dumps cameras/canvases/hierarchy to the log, saves a screenshot, exits.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AmongUsGameTools
{
	[InitializeOnLoad]
	public static class MenuCapture
	{
		const string Key = "AmongUsGame.MenuCapture";
		static int frames, target, stage;
		static string outPath;
		static float[] sampleTimes;
		static int sampleIndex;
		static double nextSample;
		static string[] actions;
		static int actionIndex;
		static bool stay;
		static MenuCapture()
		{
			if (EditorPrefs.GetBool(Key, false))
			{
				if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
					EditorApplication.delayCall += () => { EditorApplication.isPlaying = true; };
				target = EditorPrefs.GetInt(Key + ".frames", 120);
				outPath = EditorPrefs.GetString(Key + ".out", "C:/spacemafia-game/scratch/menu.png");
				string samples = EditorPrefs.GetString(Key + ".samples", "");
				if (!string.IsNullOrEmpty(samples)) sampleTimes = Array.ConvertAll(samples.Split(',', ';'), x => float.Parse(x, System.Globalization.CultureInfo.InvariantCulture));
				stay = EditorPrefs.GetBool(Key + ".stay", false);
				if (stay) EditorApplication.playModeStateChanged += st => { if (st == PlayModeStateChange.EnteredEditMode) EditorPrefs.SetBool(Key, false); }; // manual session ended: do not auto-play next time
				string actionText = EditorPrefs.GetString(Key + ".actions", "");
				if (!string.IsNullOrEmpty(actionText)) actions = actionText.Split(',');
				EditorApplication.update += Tick;
			}
		}
		public static void Start()
		{
			var kv = new Dictionary<string, string>();
			foreach (var a in Environment.GetCommandLineArgs()) { int i = a.IndexOf('='); if (i > 0 && !a.StartsWith("-")) kv[a.Substring(0, i)] = a.Substring(i + 1); }
			if (kv.TryGetValue("registry", out var registry) && registry == "true") BuildRegistry.Run();
			EditorPrefs.SetBool(Key, true);
			EditorPrefs.SetInt(Key + ".frames", kv.TryGetValue("frames", out var fr) ? int.Parse(fr) : 120);
			EditorPrefs.SetString(Key + ".out", kv.TryGetValue("out", out var o) ? o : "C:/spacemafia-game/scratch/menu.png");
			EditorPrefs.SetString(Key + ".samples", kv.TryGetValue("samples", out var sm) ? sm : "");
			EditorPrefs.SetBool(Key + ".stay", kv.TryGetValue("stay", out var stv) && stv == "true"); // stay=true: run the scheduled actions, then keep playing for manual testing (no screenshots, no exit)
			EditorPrefs.SetString(Key + ".actions", kv.TryGetValue("actions", out var ac) ? ac : "");
			EditorPrefs.SetBool("AmongUsGame.HudIcons", kv.TryGetValue("hudicons", out var hi) && hi == "true");
			string scene = kv.TryGetValue("scene", out var s) ? s : "MainMenu";
			int w = kv.TryGetValue("w", out var ws) ? int.Parse(ws) : 1920, h = kv.TryGetValue("h", out var hs) ? int.Parse(hs) : 1080;
			SetGameViewSize(w, h);
			UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/" + scene + ".unity");
			EditorApplication.isPlaying = true;
		}
		static void SetGameViewSize(int w, int h)
		{
			try
			{
				var gvT = Type.GetType("UnityEditor.GameView,UnityEditor");
				var gv = EditorWindow.GetWindow(gvT);
				var sizesT = Type.GetType("UnityEditor.GameViewSizes,UnityEditor");
				var singleT = typeof(ScriptableSingleton<>).MakeGenericType(sizesT);
				var inst = singleT.GetProperty("instance").GetValue(null);
				var group = sizesT.GetMethod("GetGroup").Invoke(inst, new object[] { 0 });
				var gsT = Type.GetType("UnityEditor.GameViewSize,UnityEditor");
				var gsType = (Type)Type.GetType("UnityEditor.GameViewSizeType,UnityEditor");
				var size = Activator.CreateInstance(gsT, new object[] { Enum.ToObject(gsType, 1), w, h, "cap" });
				group.GetType().GetMethod("AddCustomSize").Invoke(group, new object[] { size });
				int idx = (int)group.GetType().GetMethod("GetTotalCount").Invoke(group, null) - 1;
				gvT.GetProperty("selectedSizeIndex", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).SetValue(gv, idx);
				Debug.Log("[MenuCapture] game view size set " + w + "x" + h);
			}
			catch (Exception e) { Debug.Log("[MenuCapture] SetGameViewSize failed: " + e.Message); }
		}
		static void Tick()
		{
			if (!EditorApplication.isPlaying || EditorApplication.isCompiling) return;
			frames++;
			HudIconCapture.Tick(outPath);
			if (actions != null && actionIndex < actions.Length)
			{
				string entry = actions[actionIndex];
				int colon = entry.IndexOf(':');
				if (Time.time >= float.Parse(entry.Substring(0, colon), System.Globalization.CultureInfo.InvariantCulture))
				{
					actionIndex++;
					try { RunAction(entry.Substring(colon + 1)); }
					catch (Exception e) { Debug.LogException(e); }
				}
			}
			if (stay) return;
			if (sampleTimes != null)
			{
				if (sampleIndex < sampleTimes.Length && Time.time >= sampleTimes[sampleIndex] && EditorApplication.timeSinceStartup >= nextSample)
				{
					Dump();
					string samplePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(outPath), System.IO.Path.GetFileNameWithoutExtension(outPath) + "_s" + sampleIndex + ".png");
					System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(samplePath));
					ScreenCapture.CaptureScreenshot(samplePath);
					Debug.Log("[MenuCapture] sample=" + sampleIndex + " time=" + Time.time + " requested=" + samplePath);
					sampleIndex++;
					nextSample = EditorApplication.timeSinceStartup + 0.5;
				}
				if (sampleIndex == sampleTimes.Length && EditorApplication.timeSinceStartup > nextSample)
				{
					EditorPrefs.SetBool(Key, false);
					EditorApplication.isPlaying = false;
					EditorApplication.delayCall += () => EditorApplication.Exit(0);
				}
				return;
			}
			if (frames == target && stage == 0)
			{
				stage = 1;
				Dump();
				var dir = System.IO.Path.GetDirectoryName(outPath);
				System.IO.Directory.CreateDirectory(dir);
				try { System.IO.File.Delete(outPath); } catch { }
				ScreenCapture.CaptureScreenshot(outPath);
				Debug.Log("[MenuCapture] requested screenshot " + outPath);
			}
			else if (stage == 1 && frames > target + 10)
			{
				stage = 2;
				Debug.Log("[MenuCapture] exists=" + System.IO.File.Exists(outPath));
				EditorPrefs.SetBool(Key, false);
				EditorApplication.isPlaying = false;
				EditorApplication.delayCall += () => EditorApplication.Exit(0);
			}
		}
		static void RunAction(string action)
		{
			if (OnlineFlowCapture.Run(action)) return;
			if (FreeplayWaveCapture.Run(action)) return;
			if (Roles3Capture.Run(action)) return;
			Debug.Log("[MenuCapture] action=" + action + " time=" + Time.time);
			if (action == "buttons")
			{
				foreach (var pb in UnityEngine.Object.FindObjectsByType<PassiveButton>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
					Debug.Log("[MenuCapture] BTN " + pb.name + " path=" + pb.transform.parent?.name + "/" + pb.name + " pos=" + pb.transform.position.ToString("F2") + " en=" + pb.enabled);
				return;
			}
			if (action == "here")
			{
				var hp = MapBehaviour.Instance != null ? MapBehaviour.Instance.HerePoint : null;
				if (hp == null) { Debug.Log("[MenuCapture] HERE none"); return; }
				var hm2 = hp.material;
				Debug.Log("[MenuCapture] HERE sprite=" + hp.sprite.name + " tex=" + hp.sprite.texture.name + " rect=" + hp.sprite.rect + " texsize=" + hp.sprite.texture.width + "x" + hp.sprite.texture.height + " shader=" + hm2.shader.name + " back=" + hm2.GetColor("_BackColor") + " body=" + hm2.GetColor("_BodyColor") + " visor=" + hm2.GetColor("_VisorColor") + " color=" + hp.color + " mainTex=" + (hm2.GetTexture("_MainTex") ? hm2.GetTexture("_MainTex").name : "null") + " maskLayer=" + hm2.GetInt("_MaskLayer"));
				return;
			}
			if (action.StartsWith("role="))
			{
				PlayerControl.LocalPlayer.RpcSetRole((AmongUs.GameOptions.RoleTypes)Enum.Parse(typeof(AmongUs.GameOptions.RoleTypes), action.Substring(5), true));
				return;
			}
			if (action.StartsWith("tp="))
			{
				var xy = action.Substring(3).Split(';');
				PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(new Vector2(float.Parse(xy[0], System.Globalization.CultureInfo.InvariantCulture), float.Parse(xy[1], System.Globalization.CultureInfo.InvariantCulture)));
				return;
			}
			if (action.StartsWith("tpc="))
			{
				// teleport next to the Nth (name;index) active console/interactable whose GameObject name matches
				var parts = action.Substring(4).Split(';'); int idx = parts.Length > 1 ? int.Parse(parts[1]) : 0; int n = 0;
				foreach (var c in UnityEngine.Object.FindObjectsByType<Console>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).OrderBy(c => c.transform.position.x).ThenBy(c => c.transform.position.y))
					if (c.name == parts[0] && n++ == idx)
					{
						var pos = (Vector2)c.transform.position + new Vector2(0f, -0.4f);
						PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(pos);
						Debug.Log("[MenuCapture] ACT tpc " + c.name + " at " + c.transform.position.ToString("F2") + " task=" + (c.ValidTasks != null ? c.ValidTasks.Length.ToString() : "?"));
						return;
					}
				foreach (var systemConsole in UnityEngine.Object.FindObjectsByType<SystemConsole>(FindObjectsSortMode.None).OrderBy(c => c.transform.position.x).ThenBy(c => c.transform.position.y))
					if (systemConsole.name == parts[0] && n++ == idx)
					{
						PlayerControl.LocalPlayer.NetTransform.RpcSnapTo((Vector2)systemConsole.transform.position + new Vector2(0f, -0.4f));
						Debug.Log("[MenuCapture] ACT tpc system=" + systemConsole.name + " minigame=" + systemConsole.MinigamePrefab);
						return;
					}
				Debug.Log("[MenuCapture] ACT tpc console not found " + action);
				return;
			}
			if (action == "tasks")
			{
				var d = PlayerControl.LocalPlayer.Data;
				foreach (var t in PlayerControl.LocalPlayer.myTasks) { var ntp = t as NormalPlayerTask; Debug.Log("[MenuCapture] TASK " + t.GetType().Name + " type=" + (ntp != null ? t.TaskType.ToString() : "?") + " step=" + (ntp != null ? ntp.taskStep + "/" + ntp.MaxStep : "?") + " complete=" + t.IsComplete + " minigame=" + (t.MinigamePrefab ? t.MinigamePrefab.name : "null")); }
				Debug.Log("[MenuCapture] TASK total=" + GameData.Instance.TotalTasks + " done=" + GameData.Instance.CompletedTasks + " minigame=" + (Minigame.Instance ? Minigame.Instance.name : "none"));
				return;
			}
			if (action.StartsWith("mclick="))
			{
				// full simulated mouse click: hover, down graphic, up (OnClick), up graphic, (keeps hover)
				string want = action.Substring(7); PassiveButton found = null;
				foreach (var pb in UnityEngine.Object.FindObjectsByType<PassiveButton>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
					if ((pb.name == want || (pb.transform.parent != null && pb.transform.parent.name + "/" + pb.name == want)) && pb.isActiveAndEnabled) { found = pb; break; }
				if (found == null) { Debug.Log("[MenuCapture] MCLICK not found: " + want); return; }
				Debug.Log("[MenuCapture] MCLICK " + want + " calls=" + string.Join(";", Enumerable.Range(0, found.OnClick.GetPersistentEventCount()).Select(i => (found.OnClick.GetPersistentTarget(i) ? found.OnClick.GetPersistentTarget(i).GetType().Name : "null") + "." + found.OnClick.GetPersistentMethodName(i))));
				found.ReceiveMouseOver();
				if (found.HandleDownClickGraphic) found.ReceiveClickDownGraphic();
				if (found.HandleDown) found.ReceiveClickDown();
				if (found.HandleUp) found.ReceiveClickUp();
				if (found.HandleUpClickGraphic) found.ReceiveClickUpGraphic();
				return;
			}
			if (action.StartsWith("tpt="))
			{
				// teleport next to the console of the Nth (default 0) assigned task: tpt=N
				int ti = action.Length > 4 ? int.Parse(action.Substring(4)) : 0;
				var tk = PlayerControl.LocalPlayer.myTasks[ti];
				foreach (var c in UnityEngine.Object.FindObjectsByType<Console>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).OrderBy(c => c.transform.position.x))
					if (typeof(Console).GetMethod("FindTask", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public).Invoke(c, new object[] { PlayerControl.LocalPlayer }) == (object)tk)
					{
						PlayerControl.LocalPlayer.NetTransform.RpcSnapTo((Vector2)c.transform.position + new Vector2(0f, -0.4f));
						Debug.Log("[MenuCapture] ACT tpt " + tk.TaskType + " console=" + c.name + " at " + c.transform.position.ToString("F2") + " id=" + c.ConsoleId);
						return;
					}
				Debug.Log("[MenuCapture] ACT tpt no console for " + tk.TaskType);
				return;
			}
			if (action.StartsWith("consoles"))
			{
				string filter = action.Length > 9 ? action.Substring(9) : "";
				foreach (var sc in UnityEngine.Object.FindObjectsByType<SystemConsole>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
					if (sc.name.Contains(filter)) Debug.Log("[MenuCapture] CONS SystemConsole " + sc.name + " pos=" + sc.transform.position.ToString("F2") + " icon=" + sc.UseIcon + " mini=" + (sc.MinigamePrefab ? sc.MinigamePrefab.name : "null") + " dist=" + sc.usableDistance);
				return;
			}
			if (action == "platinfo" || action == "tpplat")
			{
				var lp = PlayerControl.LocalPlayer;
				foreach (var pcn in UnityEngine.Object.FindObjectsByType<PlatformConsole>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
				{
					if (action == "tpplat" && pcn.name == "PlatformLeft") { lp.NetTransform.RpcSnapTo((Vector2)pcn.transform.position + new Vector2(0f, 0.3f)); }
					bool cu, cou; float dist = pcn.CanUse(lp.Data, out cu, out cou);
					Debug.Log("[MenuCapture] PLAT camMain=" + (Camera.main ? Camera.main.name : "null") + " vibCam=" + (typeof(VibrationManager).GetField("cam", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(DestroyableSingleton<VibrationManager>.Instance) as Camera) + " " + action + " console=" + pcn.name + " pos=" + pcn.transform.position.ToString("F2") + " platPos=" + pcn.Platform.transform.position.ToString("F2") + " inUse=" + pcn.Platform.InUse + " canUse=" + cu + " couldUse=" + cou + " dist=" + dist.ToString("F2")
						+ " | player pos=" + lp.transform.position.ToString("F2") + " dead=" + lp.Data.IsDead + " canMove=" + lp.CanMove + " moveable=" + lp.moveable + " layer=" + LayerMask.LayerToName(lp.gameObject.layer) + " inMovingPlat=" + lp.inMovingPlat + " colliderEn=" + lp.Collider.enabled + " physEn=" + lp.MyPhysics.enabled + " role=" + lp.Data.RoleType + " roleCanUse=" + lp.Data.Role.CanUse(pcn) + " kinematic=" + lp.GetComponent<Rigidbody2D>().isKinematic + " simulated=" + lp.GetComponent<Rigidbody2D>().simulated);
				}
				return;
			}
			if (action == "adderhome") { UnityEngine.Object.FindFirstObjectByType<TaskAdderGame>().GoToRoot(); return; }
			if (action.StartsWith("adder="))
			{
				// click a TaskFolder (by FolderName) or TaskAddButton (by text) inside the Freeplay laptop minigame
				string want = action.Substring(6);
				foreach (var tf in UnityEngine.Object.FindObjectsByType<TaskFolder>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
					if (tf.FolderName == want && tf.Button != null && tf.transform.parent != null && tf.gameObject.activeInHierarchy && tf.Parent != null) { Debug.Log("[MenuCapture] ACT adder folder " + want); tf.Button.ReceiveClickUp(); return; }
				foreach (var tb in UnityEngine.Object.FindObjectsByType<TaskAddButton>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
					if (tb.Text != null && tb.Text.text.Contains(want) && tb.Button != null) { Debug.Log("[MenuCapture] ACT adder button " + tb.Text.text); tb.Button.ReceiveClickUp(); return; }
				Debug.Log("[MenuCapture] ACT adder not found " + want);
				return;
			}
			if (action.StartsWith("addtask="))
			{
				// add the first shipstatus task prefab of the given TaskTypes (clears all current tasks first when clear=1 was run)
				var tt = (TaskTypes)Enum.Parse(typeof(TaskTypes), action.Substring(8), true);
				var all = ShipStatus.Instance.CommonTasks.Concat(ShipStatus.Instance.LongTasks).Concat(ShipStatus.Instance.ShortTasks);
				var prefab = all.FirstOrDefault(t => t.TaskType == tt);
				if (prefab == null) { Debug.Log("[MenuCapture] ACT addtask no prefab " + tt); return; }
				TaskAddButton.AddTaskToPlayer(prefab);
				Debug.Log("[MenuCapture] ACT addtask " + tt + " now " + PlayerControl.LocalPlayer.myTasks.Count);
				return;
			}
			if (action == "cleartasks")
			{
				foreach (var t in PlayerControl.LocalPlayer.myTasks.ToArray()) PlayerControl.LocalPlayer.RemoveTask(t);
				return;
			}
			if (action.StartsWith("tptask="))
			{
				// teleport to console of the first task with the given TaskTypes
				var tt = (TaskTypes)Enum.Parse(typeof(TaskTypes), action.Substring(7), true);
				var tk = PlayerControl.LocalPlayer.myTasks.FirstOrDefault(t => t.TaskType == tt);
				if (tk == null) { Debug.Log("[MenuCapture] ACT tptask no task " + tt); return; }
				foreach (var c in UnityEngine.Object.FindObjectsByType<Console>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).OrderBy(c => c.transform.position.x))
					if (typeof(Console).GetMethod("FindTask", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public).Invoke(c, new object[] { PlayerControl.LocalPlayer }) == (object)tk)
					{
						PlayerControl.LocalPlayer.NetTransform.RpcSnapTo((Vector2)c.transform.position + new Vector2(0f, -0.4f));
						Debug.Log("[MenuCapture] ACT tptask " + tt + " console=" + c.name + " step=" + ((NormalPlayerTask)tk).taskStep);
						return;
					}
				Debug.Log("[MenuCapture] ACT tptask no console for " + tt);
				return;
			}
			if (action == "mg")
			{
				Debug.Log("[MenuCapture] ACT mg " + (Minigame.Instance ? Minigame.Instance.name : "none") + " tasks=" + PlayerControl.LocalPlayer.myTasks.Count + " total=" + GameData.Instance.TotalTasks + " done=" + GameData.Instance.CompletedTasks);
				return;
			}
			if (action.StartsWith("sprinfo="))
			{
				string pre = action.Substring(8);
				foreach (var r in UnityEngine.Object.FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
				{
					if (!r.sprite || !r.sprite.name.StartsWith(pre) || !r.gameObject.activeInHierarchy) continue;
					var s = r.sprite;
					Debug.Log("[MenuCapture] ACT sprinfo " + s.name + " draw=" + r.drawMode + " size=" + r.size + " tile=" + r.tileMode + " rect=" + s.rect + " texRect=" + s.textureRect + " border=" + s.border + " ppu=" + s.pixelsPerUnit + " packed=" + s.packed + " packMode=" + s.packingMode + " verts=" + s.vertices.Length + " tex=" + s.texture.name + " wrap=" + s.texture.wrapMode + " bounds=" + r.bounds.size.ToString("F2") + " lossy=" + r.transform.lossyScale.ToString("F2"));
				}
				return;
			}
			if (action.StartsWith("hidespr="))
			{
				string pre = action.Substring(8); int n = 0;
				foreach (var r in UnityEngine.Object.FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
					if (r.sprite && r.sprite.name.StartsWith(pre)) { r.enabled = false; n++; }
				Debug.Log("[MenuCapture] ACT hidespr " + pre + " hidden=" + n);
				return;
			}
			if (action.StartsWith("findspr="))
			{
				string pre = action.Substring(8);
				int n = 0;
				foreach (var r in UnityEngine.Object.FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
				{
					if (!r.sprite || !r.sprite.name.StartsWith(pre)) continue;
					n++;
					string path = r.name; for (var t = r.transform.parent; t != null; t = t.parent) path = t.name + "/" + path;
					Debug.Log("[MenuCapture] ACT findspr " + r.sprite.name + " active=" + r.gameObject.activeInHierarchy + " en=" + r.enabled + " pos=" + r.transform.position.ToString("F1") + " path=" + path);
				}
				Debug.Log("[MenuCapture] ACT findspr total=" + n + " prefix=" + pre);
				return;
			}
			if (action == "mgdump")
			{
				var mg = Minigame.Instance;
				if (!mg) { Debug.Log("[MenuCapture] ACT mgdump no minigame"); return; }
				var rs = mg.GetComponentsInChildren<SpriteRenderer>(true);
				Debug.Log("[MenuCapture] ACT mgdump " + mg.name + " renderers=" + rs.Length + " masks=" + mg.GetComponentsInChildren<SpriteMask>(true).Length);
				foreach (var m in mg.GetComponentsInChildren<SpriteMask>(true)) Debug.Log("[MenuCapture] ACT mgmask " + m.name + " en=" + m.enabled + " active=" + m.gameObject.activeInHierarchy + " sprite=" + (m.sprite ? m.sprite.name : "NULL") + " alphaCutoff=" + m.alphaCutoff);
				foreach (var r in rs.Take(90))
				{
					var mat = r.sharedMaterial;
					Debug.Log("[MenuCapture] ACT mgr " + r.name + " active=" + r.gameObject.activeInHierarchy + " en=" + r.enabled + " sprite=" + (r.sprite ? r.sprite.name : "NULL") + " mask=" + r.maskInteraction + " shader=" + (mat && mat.shader ? mat.shader.name + (mat.shader.isSupported ? "" : "(UNSUPPORTED)") : "NULL") + " a=" + r.color.a.ToString("F2") + " layer=" + r.sortingLayerName + "/" + r.sortingOrder + " z=" + r.transform.position.z.ToString("F1") + (mat && mat.HasProperty("_AddColor") ? " add=" + mat.GetColor("_AddColor") : "") + (mat && mat.HasProperty("_MultColor") ? " mult=" + mat.GetColor("_MultColor") : "") + (mat ? " queue=" + mat.renderQueue + " mat=" + mat.name : "") + " tex=" + (r.sprite ? r.sprite.texture.name + "/" + r.sprite.texture.width + "x" + r.sprite.texture.height : "-") + " bounds=" + r.bounds.size.ToString("F2") + " scale=" + r.transform.lossyScale.ToString("F2") + " parent=" + (r.transform.parent ? r.transform.parent.name : "-") + " pos=" + r.transform.position.ToString("F1"));
				}
				return;
			}
			if (action == "egfix")
			{
				var eg = UnityEngine.Object.FindFirstObjectByType<EmptyGarbageMinigame>();
				foreach (var m in eg.GetComponentsInChildren<SpriteMask>(true)) Debug.Log("[MenuCapture] ACT egmask " + m.name + " en=" + m.enabled + " sprite=" + (m.sprite ? m.sprite.name : "null") + " bounds=" + m.bounds.center.ToString("F2") + "/" + m.bounds.size.ToString("F2") + " cutoff=" + m.alphaCutoff + " custom=" + m.isCustomRangeActive + " order=" + m.sortingOrder + " mat=" + (m.sharedMaterial ? m.sharedMaterial.shader.name : "null"));
				foreach (var r in eg.GetComponentsInChildren<SpriteRenderer>(true)) if (r.name.Contains("(Clone)")) { if (r.maskInteraction != SpriteMaskInteraction.None) { r.maskInteraction = SpriteMaskInteraction.None; } }
				return;
			}
			if (action == "egdump")
			{
				var eg = UnityEngine.Object.FindFirstObjectByType<EmptyGarbageMinigame>();
				if (eg == null) { Debug.Log("[MenuCapture] ACT egdump none"); return; }
				var objs = (SpriteRenderer[])typeof(EmptyGarbageMinigame).GetField("Objects", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(eg);
				Debug.Log("[MenuCapture] ACT egdump n=" + objs.Length + " garbagePrefabs=" + eg.GarbagePrefabs.Length + " leaf=" + eg.LeafPrefabs.Length + " special=" + eg.SpecialObjectPrefabs.Length);
				foreach (var o in objs.Take(3)) Debug.Log("[MenuCapture] ACT eg scale=" + o.transform.lossyScale + " draw=" + o.drawMode + " active=" + o.gameObject.activeInHierarchy + " parent=" + o.transform.parent.name + " rb=" + (o.GetComponent<Rigidbody2D>() ? o.GetComponent<Rigidbody2D>().bodyType.ToString() : "none") + " layer=" + (o ? LayerMask.LayerToName(o.gameObject.layer) + " vis=" + o.isVisible : "") + " " + (o ? o.name + " local=" + o.transform.localPosition.ToString("F2") + " world=" + o.transform.position.ToString("F2") + " sprite=" + (o.sprite ? o.sprite.name : "null") + " en=" + o.enabled + " color=" + o.color + " order=" + o.sortingOrder + "/" + o.sortingLayerName + " mat=" + (o.sharedMaterial ? o.sharedMaterial.shader.name : "null") + " tex=" + (o.sprite ? o.sprite.texture.name + " " + o.sprite.texture.width + "x" + o.sprite.texture.height + " rect=" + o.sprite.rect : "") + " bounds=" + o.bounds.center.ToString("F2") + "/" + o.bounds.size.ToString("F2") : "null"));
				foreach (var cam in Camera.allCameras) Debug.Log("[MenuCapture] ACT egcam " + cam.name + " mask=" + cam.cullingMask + " UI=" + ((cam.cullingMask & (1 << LayerMask.NameToLayer("UI"))) != 0) + " UICollide=" + ((cam.cullingMask & (1 << LayerMask.NameToLayer("UICollide"))) != 0) + " pos=" + cam.transform.position.ToString("F1") + " ortho=" + cam.orthographic + " depth=" + cam.depth + " sortMode=" + cam.transparencySortMode);
				foreach (var r in eg.GetComponentsInChildren<SpriteRenderer>(false)) if (!r.name.Contains("(Clone)")) Debug.Log("[MenuCapture] ACT egr layer=" + LayerMask.LayerToName(r.gameObject.layer) + " vis=" + r.isVisible + " " + r.name + " order=" + r.sortingOrder + "/" + r.sortingLayerName + " z=" + r.transform.position.z.ToString("F2") + " en=" + r.enabled + " sprite=" + (r.sprite ? r.sprite.name : "null") + " bounds=" + r.bounds.center.ToString("F2") + "/" + r.bounds.size.ToString("F2"));
				return;
			}
			if (action == "ignorekeys") { Rewired.KeyMap.IgnorePhysical = true; return; }
			if (action.StartsWith("press="))
			{
				Rewired.KeyMap.Pressed[(int)typeof(RewiredConsts.Action).GetField(action.Substring(6)).GetRawConstantValue()] = Time.frameCount;
				return;
			}
			if (action.StartsWith("click="))
			{
				string want = action.Substring(6); PassiveButton found = null;
				foreach (var pb in UnityEngine.Object.FindObjectsByType<PassiveButton>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
					if (pb.name == want && pb.isActiveAndEnabled) { found = pb; break; }
				if (found == null) { Debug.Log("[MenuCapture] CLICK not found: " + want); return; }
				Debug.Log("[MenuCapture] CLICK " + want + " listeners=" + found.OnClick.GetPersistentEventCount());
				found.OnClick.Invoke();
				return;
			}
			if (action == "mapstate")
			{
				var mb = MapBehaviour.Instance; var hm = DestroyableSingleton<HudManager>.Instance;
				Debug.Log("[MenuCapture] MAPSTATE inst=" + (mb != null) + " open=" + (mb != null && mb.IsOpen) + " hudMapBtn=" + (hm.MapButton != null && hm.MapButton.isActiveAndEnabled) + " canMove=" + (PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.CanMove) + " introDisplayed=" + hm.IsIntroDisplayed);
				if (ShipStatus.Instance != null) Debug.Log("[MenuCapture] MAPSTATE systems=" + string.Join(",", ShipStatus.Instance.Systems.Keys.Select(k => k.ToString())) + " type=" + ShipStatus.Instance.GetType().Name);
				if (mb != null && false) foreach (var sr in mb.GetComponentsInChildren<SpriteRenderer>(false)) Debug.Log("[MenuCapture] MAPSR " + sr.name + " en=" + sr.enabled + " sprite=" + (sr.sprite ? sr.sprite.name : "null") + " pos=" + sr.transform.position.ToString("F2") + " order=" + sr.sortingOrder + " mat=" + (sr.sharedMaterial ? sr.sharedMaterial.name + "/" + sr.sharedMaterial.shader.name : "null"));
				return;
			}
			if (action == "hudicons") { HudIconCapture.Dump(DestroyableSingleton<HudManager>.Instance, "action"); return; }
			if (action.StartsWith("freeplay="))
			{
				var mapArg = action.Substring(9);
				if (string.Equals(mapArg, "Mira", StringComparison.OrdinalIgnoreCase)) mapArg = "MiraHQ"; // the enum value is MiraHQ; "Mira" threw ArgumentException so Mira captures never started a game
				var requestedMap = (MapNames)Enum.Parse(typeof(MapNames), mapArg, true);
				if (GameOptionsManager.Instance == null) { new GameOptionsManager(new InnerNet.UnityLogger()); GameOptionsManager.Instance.Initialize(); } // capture shim: the real creator lives in SplashIntro
				foreach (var mapButton in UnityEngine.Object.FindObjectsByType<FreeplayPopoverButton>(FindObjectsInactive.Include, FindObjectsSortMode.None))
					if (mapButton.map == requestedMap) { mapButton.button.OnClick.Invoke(); return; }
				throw new InvalidOperationException("Freeplay map button not found: " + requestedMap);
			}
			if (action.StartsWith("hold="))
			{
				Rewired.KeyMap.TestHeld.Add((int)typeof(RewiredConsts.Action).GetField(action.Substring(5)).GetRawConstantValue());
				return;
			}
			if (action.StartsWith("release="))
			{
				Rewired.KeyMap.TestHeld.Remove((int)typeof(RewiredConsts.Action).GetField(action.Substring(8)).GetRawConstantValue());
				return;
			}
			if (action == "hud")
			{
				foreach (var ab in UnityEngine.Object.FindObjectsByType<ActionButton>(FindObjectsInactive.Include, FindObjectsSortMode.None))
				{
					var g = (SpriteRenderer)typeof(ActionButton).GetField("graphic", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).GetValue(ab); var m = g ? g.sharedMaterial : null;
					Debug.Log("[MenuCapture] HUDBTN " + ab.name + " active=" + ab.gameObject.activeInHierarchy + " world=" + ab.transform.position.ToString("F2") + " canInteract=" + typeof(ActionButton).GetField("canInteract", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic).GetValue(ab) + " sprite=" + (g && g.sprite ? g.sprite.name : "null") + " srEn=" + (g ? g.enabled.ToString() : "null") + " color=" + (g ? g.color.ToString() : "null") + " mat=" + (m ? m.name + "#" + m.GetHashCode() : "null") + " shader=" + (m ? m.shader.name : "null") + (m && m.HasProperty("_Percent") ? " pct=" + m.GetFloat("_Percent") + " desat=" + m.GetFloat("_Desat") + " nuv=" + m.GetVector("_NormalizedUvs") + " col=" + m.GetColor("_Color") : "") + " order=" + (g ? g.sortingOrder : 0));
				}
				var hud2 = DestroyableSingleton<HudManager>.Instance;
				Debug.Log("[MenuCapture] HUD chatActive=" + hud2.Chat.gameObject.activeSelf + " chatBtnWorld=" + hud2.Chat.transform.position.ToString("F2") + " taskPanel=" + hud2.TaskPanel.transform.localPosition.ToString("F2"));
				foreach (var ap in hud2.GetComponentsInChildren<AspectPosition>(false))
					if (ap.name.Contains("Button")) Debug.Log("[MenuCapture] HUDASPECT " + ap.name + " dist=" + ap.DistanceFromEdge + " local=" + ap.transform.localPosition.ToString("F2"));
				return;
			}
			if (action.StartsWith("btnprobe="))
			{
				var hud = DestroyableSingleton<HudManager>.Instance;
				var probeField = typeof(ActionButton).GetField("graphic", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				foreach (var probeButton in new ActionButton[] { hud.PetButton, hud.ReportButton })
				{
					var graphic = (SpriteRenderer)probeField.GetValue(probeButton);
					if (action.EndsWith("texture")) graphic.material.SetTexture("_MainTex", graphic.sprite.texture);
					if (action.EndsWith("st")) { var block = new MaterialPropertyBlock(); graphic.GetPropertyBlock(block); block.SetVector("_MainTex_ST", new Vector4(1, 1, 0, 0)); graphic.SetPropertyBlock(block); }
					if (action.EndsWith("z")) { var p = graphic.transform.position; p.z = Camera.main.transform.position.z + 1; graphic.transform.position = p; }
				}
				HudIconCapture.Dump(hud, action);
				return;
			}
			if (action.StartsWith("btnshader="))
			{
				var shaderHud = DestroyableSingleton<HudManager>.Instance;
				var shaderField = typeof(ActionButton).GetField("graphic", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				var shader = AssetDatabase.LoadAssetAtPath<Shader>("Assets/Shader/DiagHud" + action.Substring(10) + ".shader");
				foreach (var shaderButton in new ActionButton[] { shaderHud.PetButton, shaderHud.ReportButton })
				{
					var shaderGraphic = (SpriteRenderer)shaderField.GetValue(shaderButton);
					var shaderMaterial = new Material(shaderGraphic.sharedMaterial);
					shaderMaterial.shader = shader;
					shaderGraphic.sharedMaterial = shaderMaterial;
				}
				HudIconCapture.Dump(shaderHud, action);
				return;
			}
			if (action.StartsWith("btnmat="))
			{
				var hudx = DestroyableSingleton<HudManager>.Instance;
				var f = typeof(ActionButton).GetField("graphic", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
				foreach (ActionButton ab in new ActionButton[] { hudx.PetButton, hudx.ReportButton })
				{
					var g = (SpriteRenderer)f.GetValue(ab);
					var t = g.sprite.texture;
					Debug.Log("[MenuCapture] BTNTEX " + ab.name + " tex=" + t.name + " " + t.width + "x" + t.height + " fmt=" + t.format + " rect=" + g.sprite.rect + " uv0=" + g.sprite.uv[0] + " bounds=" + g.bounds + " layer=" + g.gameObject.layer + " sortLayer=" + g.sortingLayerName + " cullMask=" + Camera.main.cullingMask + " go.layer=" + g.gameObject.layer);
					{ var sm = g.sharedMaterial; Debug.Log("[MenuCapture] BTNMAT " + ab.name + " queue=" + sm.renderQueue + " kw=" + string.Join(",", sm.enabledKeywords.Select(k => k.name)) + " passes=" + sm.passCount + " p0=" + sm.GetShaderPassEnabled("") + " tex=" + sm.GetTexture("_MainTex") + " hasMT=" + sm.HasProperty("_MainTex") + " pbCount=" + (g.HasPropertyBlock() ? 1 : 0)); }
					if (action.EndsWith("diff"))
					{
						var a1 = g.sharedMaterial; var b1 = new Material(a1.shader);
						for (int pi = 0; pi < a1.shader.GetPropertyCount(); pi++)
						{
							var pn = a1.shader.GetPropertyName(pi); var pt2 = a1.shader.GetPropertyType(pi);
							string va = "", vb = "";
							if (pt2 == UnityEngine.Rendering.ShaderPropertyType.Float || pt2 == UnityEngine.Rendering.ShaderPropertyType.Range) { va = a1.GetFloat(pn).ToString(); vb = b1.GetFloat(pn).ToString(); }
							else if (pt2 == UnityEngine.Rendering.ShaderPropertyType.Color) { va = a1.GetColor(pn).ToString(); vb = b1.GetColor(pn).ToString(); }
							else if (pt2 == UnityEngine.Rendering.ShaderPropertyType.Vector) { va = a1.GetVector(pn).ToString(); vb = b1.GetVector(pn).ToString(); }
							else if (pt2 == UnityEngine.Rendering.ShaderPropertyType.Texture) { va = (a1.GetTexture(pn) ? a1.GetTexture(pn).name : "null") + a1.GetTextureScale(pn) + a1.GetTextureOffset(pn); vb = (b1.GetTexture(pn) ? b1.GetTexture(pn).name : "null") + b1.GetTextureScale(pn) + b1.GetTextureOffset(pn); }
							Debug.Log("[MenuCapture] MATDIFF " + ab.name + " " + pn + " asset=" + va + " fresh=" + vb);
						}
						Debug.Log("[MenuCapture] MATDIFF " + ab.name + " queue asset=" + a1.renderQueue + " fresh=" + b1.renderQueue + " name=" + a1.name + " hideFlags=" + a1.hideFlags + " globalIllum=" + a1.globalIlluminationFlags + " doubleSided=" + a1.doubleSidedGI + " instancing=" + a1.enableInstancing);
					}
					if (action.EndsWith("exp2")) { var asset = g.sharedMaterial; Material m3; if (ab == hudx.PetButton) { m3 = new Material(asset); m3.DisableKeyword("ETC1_EXTERNAL_ALPHA"); } else { m3 = new Material(asset.shader); m3.CopyPropertiesFromMaterial(asset); } g.sharedMaterial = m3; }
					else if (action.EndsWith("exp")) { var cm = new Material(g.sharedMaterial); Debug.Log("[MenuCapture] BTNKW " + ab.name + " kw=" + string.Join(",", cm.shaderKeywords)); if (ab == hudx.PetButton) cm.SetTexture("_MainTex", Texture2D.whiteTexture); else cm.shaderKeywords = new string[0]; g.sharedMaterial = cm; }
					else if (action.EndsWith("copy")) g.sharedMaterial = new Material(g.sharedMaterial);
					else if (action.EndsWith("default")) g.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
					else if (action.EndsWith("cool")) { var m2 = new Material(g.sharedMaterial.shader); m2.SetVector("_NormalizedUvs", new Vector4(0, 1, 0, 1)); m2.SetFloat("_Percent", 0f); m2.SetColor("_Color", new Color(.5f, .5f, .5f, 1)); g.sharedMaterial = m2; }
				}
				return;
			}
			if (action == "pet")
			{
				var lpc = PlayerControl.LocalPlayer;
				Debug.Log("[MenuCapture] PET before pet=" + (lpc.cosmetics.currentPet ? lpc.cosmetics.currentPet.name : "null"));
				lpc.TryPet();
				return;
			}
			if (action == "cosdump")
			{
				var hm = DestroyableSingleton<HatManager>.Instance;
				Debug.Log("[MenuCapture] COSDUMP hats=" + hm.allHats.Length + " visors=" + hm.allVisors.Length + " skins=" + hm.allSkins.Length + " pets=" + hm.allPets.Length);
				foreach (var h in hm.allHats) Debug.Log("[MenuCapture] COSHAT " + h.ProductId + " inFront=" + h.InFront + " noBounce=" + h.NoBounce);
				foreach (var v in hm.allVisors) Debug.Log("[MenuCapture] COSVISOR " + v.ProductId);
				foreach (var sk in hm.allSkins) Debug.Log("[MenuCapture] COSSKIN " + sk.ProductId);
				foreach (var pt in hm.allPets) Debug.Log("[MenuCapture] COSPET " + pt.ProductId);
				return;
			}
			if (action.StartsWith("cos="))
			{
				// cos=hat:ID;visor:ID;skin:ID;pet:ID  (applied locally to the local player, colour 7)
				var lp2 = PlayerControl.LocalPlayer;
				foreach (var part in action.Substring(4).Split(';'))
				{
					var kv2 = part.Split(':');
					int col = lp2.Data.DefaultOutfit.ColorId;
					if (kv2[0] == "hat") lp2.SetHat(kv2[1], col);
					else if (kv2[0] == "visor") lp2.SetVisor(kv2[1], col);
					else if (kv2[0] == "skin") lp2.SetSkin(kv2[1], col);
					else if (kv2[0] == "pet") lp2.SetPet(kv2[1], col);
				}
				return;
			}
			if (action == "pos")
			{
				var lpp = PlayerControl.LocalPlayer;
				if (lpp)
				{
					foreach (var sr in lpp.cosmetics.GetComponentsInChildren<SpriteRenderer>(true))
					{
						var ns = sr.GetComponent<PowerTools.SpriteAnimNodeSync>();
						Debug.Log("[MenuCapture] SR " + sr.transform.parent.name + "/" + sr.name + " en=" + sr.enabled + (sr.gameObject.activeInHierarchy ? "" : "(inactive)") + " local=" + sr.transform.localPosition.ToString("F3") + " world=" + sr.transform.position.ToString("F3") + " sprite=" + (sr.sprite ? sr.sprite.name : "null") + " flip=" + sr.flipX + " order=" + sr.sortingOrder + " layer=" + sr.sortingLayerName + " mat=" + (sr.sharedMaterial ? sr.sharedMaterial.name : "null") + (ns ? " NODE id=" + ns.NodeId + " en=" + ns.enabled + " n=" + ns.normalOffset + " f=" + ns.flipOffset + " parent=" + (ns.Parent ? ns.Parent.name : "null") : ""));
					}
					var rb = lpp.GetComponent<Rigidbody2D>();
					var hud = DestroyableSingleton<HudManager>.Instance;
					Debug.Log("[MenuCapture] MOVE vel=" + (rb ? rb.linearVelocity.ToString() : "null") + " canMove=" + lpp.CanMove + " joyDelta=" + (hud && hud.joystick != null ? hud.joystick.DeltaL.ToString() : "null") + " joy=" + (hud && hud.joystick != null ? hud.joystick.GetType().Name : "null") + " keys=" + string.Join("", new[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow }.Where(k => Input.GetKey(k)).Select(k => k.ToString() + ",")) + " held=" + string.Join(",", Rewired.KeyMap.TestHeld));
					var o = lpp.cosmetics; Debug.Log("[MenuCapture] OUTFIT hat=" + (o.GetComponentInChildren<HatParent>() != null && o.GetComponentInChildren<HatParent>().Hat != null ? o.GetComponentInChildren<HatParent>().Hat.ProductId : "null") + " visor=" + (o.CurrentVisor != null && o.CurrentVisor.visorData != null ? o.CurrentVisor.visorData.ProductId : "null") + " pet=" + (o.currentPet != null ? o.currentPet.name : "null") + " skin=" + (o.skin != null && o.skin.data != null ? o.skin.data.ProductId : "null"));
					var anim = lpp.MyPhysics.Animations;
					Debug.Log("[MenuCapture] ANIM run=" + anim.IsPlayingRunAnimation() + " idle=" + anim.IsPlayingSomeAnimation() + " flip=" + lpp.cosmetics.FlipX);
				}
				var lp = PlayerControl.LocalPlayer;
				Debug.Log("[MenuCapture] POS local=" + (lp ? lp.transform.position.ToString() : "null") + " cam=" + Camera.main.transform.position + " role=" + (lp && lp.Data != null && lp.Data.Role != null ? lp.Data.Role.Role.ToString() : "null") + " name=" + (lp && lp.Data != null ? lp.Data.PlayerName : "null") + " colorId=" + (lp && lp.Data != null ? lp.Data.DefaultOutfit.ColorId.ToString() : "null"));
				return;
			}
			if (action.StartsWith("mask="))
			{
				var diagnosticMenu = UnityEngine.Object.FindFirstObjectByType<MainMenuManager>();
				var diagnosticField = typeof(MainMenuManager).GetField("rightPanelMask", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				var mask = (GameObject)diagnosticField.GetValue(diagnosticMenu);
				mask.SetActive(action.Substring(5) == "on");
				Debug.Log("[MenuCapture] diagnostic mask active=" + mask.activeSelf);
				return;
			}
			if (action == "settingscheck")
			{
				var settings = AmongUs.Data.DataManager.Settings;
				var options = UnityEngine.Object.FindFirstObjectByType<OptionsMenuBehaviour>(FindObjectsInactive.Include);
				float originalSfx = settings.audio.SfxVolume, originalMusic = settings.audio.MusicVolume;
				bool originalCensor = settings.multiplayer.CensorChat;
				options.SoundSlider.Value = .35f;
				options.UpdateSfxVolume();
				options.MusicSlider.Value = .75f;
				options.UpdateMusicVolume();
				options.ToggleCensorChat();
				Debug.Log("[MenuCapture] SETTINGS beforeSave dirty=" + settings.IsDirty);
				settings.Save();
				settings.ForceLoad();
				Debug.Log("[MenuCapture] SETTINGS loaded sfx=" + settings.audio.SfxVolume + " music=" + settings.audio.MusicVolume + " censor=" + settings.multiplayer.CensorChat + " expected=" + !originalCensor);
				settings.audio.SfxVolume = originalSfx;
				settings.audio.MusicVolume = originalMusic;
				settings.multiplayer.CensorChat = originalCensor;
				settings.Save();
				options.UpdateButtons();
				return;
			}
			if (action == "languages")
			{
				foreach (var language in DestroyableSingleton<TranslationController>.Instance.Languages)
				{
					var unit = new LanguageUnit(language.Value);
					Debug.Log("[MenuCapture] LANGUAGE " + language.Key + " bytes=" + language.Value.Data.bytes.Length + " Play=" + unit.GetString("MenuPlay", Array.Empty<object>()));
				}
				return;
			}
			if (action.StartsWith("lang="))
			{
				DestroyableSingleton<TranslationController>.Instance.SetLanguage((SupportedLangs)Enum.Parse(typeof(SupportedLangs), action.Substring(5)));
				return;
			}
			if (action.StartsWith("resize="))
			{
				string[] size = action.Substring(7).Split('x');
				SetGameViewSize(int.Parse(size[0]), int.Parse(size[1]));
				ResolutionManager.SetResolution(int.Parse(size[0]), int.Parse(size[1]), false);
				return;
			}
			var menu = UnityEngine.Object.FindFirstObjectByType<MainMenuManager>();
			const System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;
			if (action == "hover")
			{
				var targetButton = (PassiveButton)menu.GetType().GetField("playButton", flags).GetValue(menu);
				var manager = DestroyableSingleton<PassiveButtonManager>.Instance;
				var over = manager.GetType().GetMethod("HandleMouseOver", flags);
				var exit = manager.GetType().GetMethod("HandleMouseOut", flags);
				var current = manager.GetType().GetField("currentOver", flags);
				current.SetValue(manager, null);
				int enters = 0, exits = 0;
				UnityEngine.Events.UnityAction onEnter = () => enters++;
				UnityEngine.Events.UnityAction onExit = () => exits++;
				targetButton.OnMouseOver.AddListener(onEnter);
				targetButton.OnMouseOut.AddListener(onExit);
				var counter = typeof(SoundManager).GetField("CapturePlaySoundCalls");
				int start = counter == null ? -1 : (int)counter.GetValue(null);
				var collider = targetButton.Colliders[0];
				var touch = manager.controller.Touches[0];
				touch.active = true;
				touch.Position = collider.bounds.center;
				for (int i = 0; i < 20; i++) over.Invoke(manager, new object[] { targetButton, collider });
				int held = counter == null ? -1 : (int)counter.GetValue(null) - start;
				touch.Position = new Vector2(1000, 1000);
				for (int i = 0; i < 20; i++) exit.Invoke(manager, null);
				touch.Position = collider.bounds.center;
				for (int i = 0; i < 20; i++) over.Invoke(manager, new object[] { targetButton, collider });
				int repeated = counter == null ? -1 : (int)counter.GetValue(null) - start;
				Debug.Log("[MenuCapture] HOVER enters=" + enters + " exits=" + exits + " soundCallsAfter20Held=" + held + " soundCallsAfterReenter=" + repeated);
				targetButton.OnMouseOver.RemoveListener(onEnter);
				targetButton.OnMouseOut.RemoveListener(onExit);
				return;
			}
			if (action.EndsWith("()"))
			{
				string method = action.Substring(0, action.Length - 2);
				object targetObject = menu;
				int dot = method.IndexOf('.');
				if (dot >= 0)
				{
					Type componentType = typeof(MainMenuManager).Assembly.GetType(method.Substring(0, dot));
					targetObject = UnityEngine.Object.FindFirstObjectByType(componentType, FindObjectsInactive.Include);
					method = method.Substring(dot + 1);
				}
				targetObject.GetType().GetMethod(method, flags).Invoke(targetObject, null);
				return;
			}
			var field = menu.GetType().GetField(action, flags);
			var button = field != null ? field.GetValue(menu) as PassiveButton : null;
			if (button == null)
				foreach (var candidate in UnityEngine.Object.FindObjectsByType<PassiveButton>(FindObjectsInactive.Exclude))
					if (Path(candidate.transform) == action || candidate.name == action) { button = candidate; break; }
			if (button == null) throw new InvalidOperationException("Capture action target not found: " + action);
			Debug.Log("[MenuCapture] click " + Path(button.transform) + " persistent=" + button.OnClick.GetPersistentEventCount());
			button.OnClick.Invoke();
		}
		static string Path(Transform t) { var sb = new StringBuilder(t.name); for (var p = t.parent; p != null; p = p.parent) sb.Insert(0, p.name + "/"); return sb.ToString(); }
		static void Tree(StringBuilder sb, Transform t, int d)
		{
			sb.AppendLine(new string(' ', d * 2) + "T " + t.name + (t.gameObject.activeSelf ? "" : " [inactive]") + " pos=" + t.position + " scale=" + t.localScale);
			if (d >= 3) return;
			for (int i = 0; i < t.childCount; i++) Tree(sb, t.GetChild(i), d + 1);
		}
		static void Dump()
		{
			var sb = new StringBuilder("[MenuCapture] ===== DUMP =====\n");
			sb.AppendLine("Time " + Time.time + " frame=" + Time.frameCount);
			sb.AppendLine("Screen " + Screen.width + "x" + Screen.height + " aspect " + (float)Screen.width / Screen.height);
			foreach (var c in UnityEngine.Object.FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None))
				sb.AppendLine("CAM " + Path(c.transform) + " active=" + c.gameObject.activeInHierarchy + " en=" + c.enabled + " ortho=" + c.orthographic + " size=" + c.orthographicSize + " fov=" + c.fieldOfView + " rect=" + c.rect + " depth=" + c.depth + " mask=" + c.cullingMask + " pos=" + c.transform.position + " tex=" + c.targetTexture + " pixel=" + c.pixelWidth + "x" + c.pixelHeight);
			foreach (var c in UnityEngine.Object.FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				var cs = c.GetComponent<UnityEngine.UI.CanvasScaler>();
				sb.AppendLine("CANVAS " + Path(c.transform) + " active=" + c.gameObject.activeInHierarchy + " mode=" + c.renderMode + " root=" + c.isRootCanvas + " scaler=" + (cs ? cs.uiScaleMode + " ref=" + cs.referenceResolution : "none") + " scale=" + c.transform.localScale);
			}
			foreach (var t in UnityEngine.Object.FindObjectsByType<TMPro.TMP_Text>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
				sb.AppendLine("TMP " + Path(t.transform) + " text='" + t.text + "'");
			foreach (var t in UnityEngine.Object.FindObjectsByType<UnityEngine.UI.Text>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
				sb.AppendLine("UITEXT " + Path(t.transform) + " text='" + t.text + "'");
			sb.AppendLine("--- root objects per scene");
			for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
			{
				var sc = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
				sb.AppendLine("SCENE " + sc.name + " loaded=" + sc.isLoaded);
				foreach (var r in sc.GetRootGameObjects())
					sb.AppendLine("  ROOT " + r.name + " active=" + r.activeSelf + " pos=" + r.transform.position + " scale=" + r.transform.localScale + " children=" + r.transform.childCount);
			}
			sb.AppendLine("--- tree (depth<=3)");
			for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
				foreach (var r in UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).GetRootGameObjects()) Tree(sb, r.transform, 0);
			int n = 0;
			foreach (var r in UnityEngine.Object.FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
			{ if (n++ < 500) sb.AppendLine("SR " + Path(r.transform) + " sprite=" + (r.sprite ? r.sprite.name : "null") + " bounds=" + r.bounds.size + " pos=" + r.transform.position + " material=" + r.sharedMaterial + " shader=" + (r.sharedMaterial ? r.sharedMaterial.shader : null) + " supported=" + (r.sharedMaterial && r.sharedMaterial.shader && r.sharedMaterial.shader.isSupported)); }
			foreach (var r in UnityEngine.Object.FindObjectsByType<MeshRenderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
				sb.AppendLine("MR " + Path(r.transform) + " bounds=" + r.bounds + " material=" + r.sharedMaterial + " shader=" + (r.sharedMaterial ? r.sharedMaterial.shader : null));
			foreach (var star in UnityEngine.Object.FindObjectsByType<StarGen>(FindObjectsInactive.Exclude))
			{
				var mesh = star.GetComponent<MeshFilter>().sharedMesh;
				sb.AppendLine("STAR " + Path(star.transform) + " vertices=" + mesh.vertexCount + " first=" + mesh.vertices[0]);
			}
			sb.AppendLine("SR total " + n);
			Debug.Log(sb.ToString());
		}
	}
}
#endif
