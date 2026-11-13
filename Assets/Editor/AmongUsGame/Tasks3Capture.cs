#if UNITY_EDITOR
// tasks3 capture helpers (editor only). Drives the REAL minigame input paths by overriding Controller.CaptureUpdate for one minigame controller
// (pointer position, TouchStart/TouchEnd, screen coordinates) so that CheckDrag-based minigames are exercised as a player would.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmongUsGameTools
{
	public static class Tasks3Capture
	{
		const BindingFlags Any = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

		public static bool Run(string action)
		{
			// t3assist=Type;Type : same as t3tour but the simulator is skipped and the minigame stays open up to 180 s so a human can finish it with the real mouse/keyboard
			if (action == "t3unskip") { IgnoreVerified = true; return true; } // visual re-check of tasks that are on scratch/verified_tasks.txt
			if (action.StartsWith("t3assist=")) { Assist = true; HudManager.Instance.StartCoroutine(Tour(action.Substring(9).Split(';'))); return true; }
			if (action.StartsWith("t3tour=")) { HudManager.Instance.StartCoroutine(Tour(action.Substring(7).Split(';'))); return true; }
			if (action.StartsWith("t3open=")) { HudManager.Instance.StartCoroutine(OpenOnly(action.Substring(7))); return true; }
			if (action == "t3state") { LogState("state"); return true; }
			if (action == "t3close") { if (Minigame.Instance) Minigame.Instance.Close(); return true; }
			return false;
		}

		static void LogState(string tag)
		{
			var hud = HudManager.Instance;
			Debug.Log("[WAVE] T3 t=" + Time.time.ToString("F1") + " " + tag + " minigame=" + (Minigame.Instance ? Minigame.Instance.GetType().Name : "none")
				+ " map=" + (MapBehaviour.Instance && MapBehaviour.Instance.gameObject.activeSelf)
				+ " menu=" + (hud && hud.GameMenu && hud.GameMenu.IsOpen)
				+ " info=" + (MatchInfoGuide.Instance && MatchInfoGuide.Instance.IsActive)
				+ " laptop=" + (PlayerCustomizationMenu.Instance != null)
				+ " canMove=" + (PlayerControl.LocalPlayer && PlayerControl.LocalPlayer.CanMove)
				+ " meeting=" + (MeetingHud.Instance != null));
		}

		class Sim
		{
			public Controller c; public Vector2 pos; public bool down, prev;
			public void Install()
			{
				Controller.CaptureUpdate = input =>
				{
					if (input != c) return false;
					var t = input.Touches[0];
					t.Position = pos;
					t.ScreenPosition = Camera.main.WorldToScreenPoint(pos);
					t.TouchStart = down && !prev;
					t.TouchEnd = !down && prev;
					t.IsDown = down;
					t.active = true;
					if (t.TouchStart) { t.DownAt = pos; t.ScreenDownAt = t.ScreenPosition; }
					prev = down;
					return true;
				};
			}
			public void Uninstall() { Controller.CaptureUpdate = null; }
			public IEnumerator Move(Vector2 to, float secs)
			{
				Vector2 from = pos;
				for (float e = 0; e < secs; e += Time.deltaTime) { pos = Vector2.Lerp(from, to, e / secs); yield return null; }
				pos = to; yield return null;
			}
			public IEnumerator Drag(IList<Vector2> pts, float seg, float hold = 0.1f)
			{
				pos = pts[0]; down = true; yield return new WaitForSeconds(0.15f);
				for (int i = 1; i < pts.Count; i++) yield return Move(pts[i], seg);
				yield return new WaitForSeconds(hold);
				down = false; yield return new WaitForSeconds(0.2f);
			}
		}

		static Controller FindController(Minigame game)
		{
			for (Type t = game.GetType(); t != null; t = t.BaseType)
				foreach (var f in t.GetFields(Any | BindingFlags.DeclaredOnly))
					if (f.FieldType == typeof(Controller)) return (Controller)f.GetValue(game);
			return null;
		}
		static T Field<T>(object o, string name)
		{
			for (Type t = o.GetType(); t != null; t = t.BaseType)
			{
				var f = t.GetField(name, Any | BindingFlags.DeclaredOnly);
				if (f != null) return (T)f.GetValue(o);
			}
			throw new Exception("no field " + name + " on " + o.GetType().Name);
		}

		static IEnumerator OpenOnly(string name)
		{
			if (Minigame.Instance) Minigame.Instance.Close();
			yield return new WaitForSeconds(.5f);
			var type = (TaskTypes)Enum.Parse(typeof(TaskTypes), name);
			var task = EnsureTask(type);
			var console = ConsolesFor(task).FirstOrDefault();
			if (!console) { Debug.Log("[WAVE] T3 open: no console " + name); yield break; }
			PlayerControl.LocalPlayer.NetTransform.RpcSnapTo((Vector2)console.transform.position + new Vector2(0, -.4f));
			yield return new WaitForSeconds(.5f);
			console.Use();
			yield return new WaitForSeconds(1);
			LogState("opened " + name);
		}

		static NormalPlayerTask EnsureTask(TaskTypes type)
		{
			var task = PlayerControl.LocalPlayer.myTasks.OfType<NormalPlayerTask>().FirstOrDefault(t => t.TaskType == type && !t.IsComplete);
			if (task) return task;
			var prefab = ShipStatus.Instance.CommonTasks.Concat(ShipStatus.Instance.LongTasks).Concat(ShipStatus.Instance.ShortTasks).FirstOrDefault(t => t.TaskType == type);
			if (!prefab) return null;
			TaskAddButton.AddTaskToPlayer(prefab);
			return PlayerControl.LocalPlayer.myTasks.OfType<NormalPlayerTask>().Last(t => t.TaskType == type);
		}

		static List<Console> ConsolesFor(NormalPlayerTask task)
		{
			var findTask = typeof(Console).GetMethod("FindTask", Any);
			return UnityEngine.Object.FindObjectsByType<Console>(FindObjectsSortMode.None)
				.Where(c => findTask.Invoke(c, new object[] { PlayerControl.LocalPlayer }) == (object)task)
				.OrderBy(c => c.ConsoleId).ThenBy(c => c.name).ToList();
		}

		// scratch/verified_tasks.txt: one "Map:Task" (or "*:Task") per line; tasks the user confirmed by hand are skipped by tour and assist mode
		static bool IgnoreVerified;
		static bool IsVerified(string task)
		{
			if (IgnoreVerified) return false;
			try
			{
				string path = "C:/spacemafia-game/scratch/verified_tasks.txt";
				if (!System.IO.File.Exists(path)) return false;
				string status = ShipStatus.Instance ? ShipStatus.Instance.GetType().Name : "";
				string map = status.Contains("Airship") ? "Airship" : status.Replace("ShipStatus", "");
				foreach (string raw in System.IO.File.ReadAllLines(path))
				{
					string line = raw.Trim();
					if (line.Length == 0 || line.StartsWith("#")) continue;
					int c = line.IndexOf(':');
					string m = c < 0 ? "*" : line.Substring(0, c), t = c < 0 ? line : line.Substring(c + 1);
					if (t == task && (m == "*" || string.Equals(m, map, StringComparison.OrdinalIgnoreCase))) return true;
				}
			}
			catch (Exception) { }
			return false;
		}
		static bool Assist;
		static IEnumerator NoSim() { yield break; }
		static IEnumerator Tour(string[] types)
		{
			foreach (string name in types)
			{
				if (IsVerified(name)) { Debug.Log("[WAVE] T3 SKIP " + name + " (already verified by the user, see scratch/verified_tasks.txt)"); continue; }
				if (Minigame.Instance) Minigame.Instance.Close();
				yield return new WaitForSeconds(.5f);
				var type = (TaskTypes)Enum.Parse(typeof(TaskTypes), name);
				var task = EnsureTask(type);
				if (!task) { Debug.Log("[WAVE] T3 missing prefab " + name); continue; }
				Debug.Log("[WAVE] T3 start " + name + " step=" + task.taskStep + "/" + task.MaxStep + " data=" + BitConverter.ToString(task.Data ?? new byte[0]));
				int stalled = 0;
				for (int pass = 0; pass < 8 && !task.IsComplete && stalled < 2; pass++)
				{
					bool any = false; int stepBefore = task.taskStep; int dataSum = (task.Data ?? new byte[0]).Sum(b => (int)b);
					// assist mode fallback: Console.FindTask does not map every task to its console (e.g. Polus InsertKeys / ScanBoardingPass), so the tour would neither open it nor move the player. Teleport to a console that lists the task type instead.
					if (Assist && ConsolesFor(task).Count == 0)
					{
						var near = UnityEngine.Object.FindObjectsByType<Console>(FindObjectsSortMode.None).Where(c => c.TaskTypes != null && c.TaskTypes.Contains(type)).OrderBy(c => c.name).FirstOrDefault();
						if (near)
						{
							PlayerControl.LocalPlayer.NetTransform.RpcSnapTo((Vector2)near.transform.position + new Vector2(0, -.4f));
							Debug.Log("[WAVE] T3 ASSIST " + name + ": teleported next to " + near.name + " - use it yourself");
						}
						else Debug.Log("[WAVE] T3 ASSIST " + name + ": no console lists this task type; find it yourself in the world");
					}
					foreach (var console in ConsolesFor(task))
					{
						Vector2 snapPos = (Vector2)console.transform.position + new Vector2(0, -.4f);
						// SubmitScan: the minigame walks the player to the pad (WalkPlayerTo is velocity based), the generic spot below the console sits behind the MedBay wall and can never reach the pad
						if (name == "SubmitScan" && ShipStatus.Instance.MedScanner) snapPos = (Vector2)ShipStatus.Instance.MedScanner.Position + new Vector2(0, .3f);
						PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(snapPos);
						yield return new WaitForSeconds(.5f);
						try { console.Use(); } catch (Exception ex) { Debug.LogException(ex); }
						yield return new WaitForSeconds(1f);
						var game = Minigame.Instance;
						if (!game) continue;
						any = true;
						Debug.Log("[WAVE] T3 open " + name + " pass=" + pass + " console=" + console.name + "#" + console.ConsoleId + " game=" + game.GetType().Name + " step=" + task.taskStep);
						if (pass == 0) UnityEngine.ScreenCapture.CaptureScreenshot("C:/spacemafia-game/scratch/shots/t3_" + name + ".png");
						if (Assist) Debug.Log("[WAVE] T3 ASSIST " + name + ": complete this task yourself with the real mouse/keyboard (waiting up to 180 s)");
						var it = Assist ? NoSim() : Interact(game, task);
						while (true)
						{
							bool next;
							try { next = it.MoveNext(); } catch (Exception ex) { Debug.LogException(ex); break; }
							if (!next) break;
							yield return it.Current;
						}
						Controller.CaptureUpdate = null;
						// the minigame must close itself after completion (original: CoStartClose / CoDestroySelf)
						float wait = 0;
						while (Minigame.Instance && wait < (Assist ? 180f : 4f)) { wait += Time.deltaTime; yield return null; }
						Debug.Log("[WAVE] T3 result " + name + " step=" + task.taskStep + "/" + task.MaxStep + " complete=" + task.IsComplete + " selfClosed=" + (!Minigame.Instance) + " waited=" + wait.ToString("F1") + " done=" + GameData.Instance.CompletedTasks + "/" + GameData.Instance.TotalTasks);
						if (Minigame.Instance) { Minigame.Instance.Close(); yield return new WaitForSeconds(.6f); }
						Debug.Log("[WAVE] T3 closed " + name + " canMove=" + PlayerControl.LocalPlayer.CanMove + " instance=" + (Minigame.Instance ? Minigame.Instance.name : "none"));
						break;
					}
					if (!any) break;
					if (task.taskStep == stepBefore && (task.Data ?? new byte[0]).Sum(b => (int)b) == dataSum) stalled++; else stalled = 0;
				}
				// assist mode: if no console could be opened automatically (tour found none), let the human do it in the world instead of reporting a false failure
				if (Assist && !task.IsComplete)
				{
					Debug.Log("[WAVE] T3 ASSIST " + name + ": could not be opened automatically - do it yourself in the world (waiting up to 120 s)");
					float aw2 = 0;
					while (!task.IsComplete && aw2 < 120f) { aw2 += Time.deltaTime; yield return null; }
				}
				Debug.Log("[WAVE] T3 end " + name + " complete=" + task.IsComplete + " step=" + task.taskStep);
			}
			Debug.Log("[WAVE] T3 FINISHED");
		}

		static IEnumerator Interact(Minigame game, NormalPlayerTask task)
		{
			if (game is CardSlideGame card) yield return Swipe(card);
			else if (game is WireMinigame wires)
			{
				var expected = Field<sbyte[]>(wires, "ExpectedWires");
				var sim = new Sim { c = FindController(wires) }; sim.Install();
				foreach (var left in wires.LeftNodes)
				{
					var right = wires.RightNodes.FirstOrDefault(r => r.WireId == expected[left.WireId]);
					if (!right) continue;
					yield return sim.Drag(new[] { (Vector2)left.hitbox.bounds.center, (Vector2)right.transform.position }, 0.5f);
				}
				yield return new WaitForSeconds(1.5f);
			}
			else if (game is DivertPowerMinigame dp)
			{
				int idx = Field<int>(dp, "sliderId");
				var col = dp.Sliders[idx];
				var sim = new Sim { c = FindController(dp) }; sim.Install();
				Vector2 start = col.bounds.center;
				Vector2 end = new Vector2(start.x, col.transform.parent.position.y + dp.SliderY.max + 0.3f);
				yield return sim.Drag(new[] { start, end }, 0.8f);
				yield return new WaitForSeconds(1.5f);
			}
			else if (game is AcceptDivertPowerGame acc) { acc.DoSwitch(); yield return new WaitForSeconds(2f); }
			else if (game is AlignGame align)
			{
				var sim = new Sim { c = FindController(align) }; sim.Install();
				Vector2 start = align.col.bounds.center;
				yield return sim.Drag(new[] { start, new Vector2(start.x, align.transform.position.y) }, 0.8f);
				yield return new WaitForSeconds(2.5f);
			}
			else if (game is LeafMinigame leaf)
			{
				var leaves = Field<Collider2D[]>(leaf, "Leaves");
				var sim = new Sim { c = FindController(leaf) }; sim.Install();
				foreach (var l in leaves.ToArray())
				{
					if (!l) continue;
					Vector2 target = (Vector2)leaf.transform.position + new Vector2(-3.6f, l.transform.localPosition.y);
					float end = Time.time + 8f;
					sim.pos = l.bounds.center; sim.down = true; yield return new WaitForSeconds(0.15f);
					while (l && Time.time < end)
					{
						sim.pos = target; yield return null;
					}
					sim.down = false; yield return new WaitForSeconds(0.2f);
				}
				yield return new WaitForSeconds(1.5f);
			}
			else if (game is CourseMinigame course)
			{
				var pts = course.PathPoints.Select(p => (Vector2)course.transform.TransformPoint(p)).ToList();
				var sim = new Sim { c = course.myController }; sim.Install();
				yield return sim.Drag(pts, 0.7f);
				yield return new WaitForSeconds(2f);
			}
			else if (game is NavigationMinigame nav)
			{
				var sim = new Sim { c = FindController(nav) }; sim.Install();
				Vector2 start = nav.hitbox.bounds.center;
				yield return sim.Drag(new[] { (Vector2)nav.CrossHairImage.transform.position, (Vector2)nav.TwoAxisImage.transform.position }, 0.8f);
				yield return new WaitForSeconds(2f);
			}
			else if (game is AdjustSteeringGame steer)
			{
				var sim = new Sim { c = FindController(steer) }; sim.Install();
				float ty = Field<float>(steer, "TargetThrustY");
				Vector2 ts = steer.Thrust.bounds.center;
				yield return sim.Drag(new[] { ts, new Vector2(ts.x, steer.transform.position.y + ty) }, 0.8f);
				float tr = Field<float>(steer, "TargetSteeringRot");
				float cur = steer.Steering.transform.localEulerAngles.z; if (cur > 180) cur -= 360;
				float dx = (cur - tr) / 15f;
				Vector2 ss = steer.Steering.bounds.center;
				yield return sim.Drag(new[] { ss, ss + new Vector2(dx, 0) }, 0.8f);
				yield return new WaitForSeconds(2f);
			}
			else if (game is EmptyGarbageMinigame garbage)
			{
				var sim = new Sim { c = FindController(garbage) }; sim.Install();
				Vector2 start = garbage.Handle.bounds.center;
				float low = garbage.transform.position.y + garbage.HandleRange.min - 0.2f;
				sim.pos = start; sim.down = true; yield return new WaitForSeconds(0.15f);
				yield return sim.Move(new Vector2(start.x, garbage.transform.position.y), 0.5f);
				yield return sim.Move(new Vector2(start.x, low), 0.6f);
				yield return new WaitForSeconds(5f);
				sim.down = false; yield return new WaitForSeconds(0.5f);
				var objs = Field<SpriteRenderer[]>(garbage, "Objects");
				Debug.Log("[WAVE] T3 garbage remaining=" + objs.Count(o => o));
				yield return new WaitForSeconds(1.5f);
			}
			else if (game is MultistageMinigame ms)
			{
				var stage = Field<Minigame>(ms, "stage");
				if (stage is RefuelStage fuel)
				{
					fuel.Refuel();
					yield return new WaitForSeconds(fuel.RefuelDuration + 1f);
					fuel.Refuel();
					yield return new WaitForSeconds(1.5f);
				}
				else { Debug.Log("[WAVE] T3 multistage stage " + (stage ? stage.GetType().Name : "null")); yield return new WaitForSeconds(.5f); }
			}
			else if (game is SimonSaysGame simon)
			{
				for (int round = 0; round < 8 && !task.IsComplete && simon; round++)
				{
					yield return new WaitForSeconds(1.2f);
					int count = task.Data[0];
					yield return new WaitForSeconds(count * 0.4f + 0.8f);
					for (int i = 0; i < count && simon && !task.IsComplete; i++)
					{
						simon.HitButton(task.Data[i + 1]);
						yield return new WaitForSeconds(0.35f);
					}
					Debug.Log("[WAVE] T3 simon round " + round + " count=" + count + " step=" + task.taskStep + "/" + task.MaxStep);
				}
				yield return new WaitForSeconds(1f);
			}
			else if (game is SampleMinigame sample)
			{
				sample.NextStep();
				yield return new WaitForSeconds(2f);
				task.TaskTimer = 1f; // test only: skip the ~15 s processing wait
				float end = Time.time + 6f;
				while (Time.time < end && task.TimerStarted != NormalPlayerTask.TimerState.Finished) yield return null;
				yield return new WaitForSeconds(1.5f);
				UnityEngine.ScreenCapture.CaptureScreenshot("C:/spacemafia-game/scratch/shots/t3_sample_select.png");
				Debug.Log("[WAVE] T3 sample timer=" + task.TimerStarted + " minigameOpen=" + (bool)Minigame.Instance + " data0=" + task.Data[0] + " data1=" + task.Data[1]);
				if (sample) sample.SelectTube(task.Data[1]);
				yield return new WaitForSeconds(4f);
			}
			else if (game is UnlockManifoldsMinigame unlock)
			{
				for (int i = 0; i < unlock.Buttons.Length && unlock; i++) { unlock.HitButton(i); yield return new WaitForSeconds(0.2f); }
				yield return new WaitForSeconds(1.5f);
			}
			else if (game is MedScanMinigame scan) { yield return new WaitForSeconds(scan.ScanDuration + 6f); }
			else
			{
				var m = typeof(FreeplayWaveCapture).GetMethod("Interact", BindingFlags.Static | BindingFlags.NonPublic);
				var it = (IEnumerator)m.Invoke(null, new object[] { game, task });
				while (it.MoveNext()) yield return it.Current;
			}
		}

		static IEnumerator Swipe(CardSlideGame game)
		{
			var sim = new Sim { c = FindController(game) }; sim.Install();
			var range = Field<FloatRange>(game, "XRange");
			Vector2 center = game.col.bounds.center;
			sim.pos = center; sim.down = true; yield return new WaitForSeconds(0.1f);
			sim.down = false; yield return new WaitForSeconds(0.8f);
			center = game.col.bounds.center;
			sim.pos = center; sim.down = true; yield return new WaitForSeconds(0.1f);
			float duration = (game.AcceptedTime.min + game.AcceptedTime.max) / 2f;
			float y = game.col.transform.localPosition.y;
			for (float e = 0; e < duration; e += Time.deltaTime)
			{
				sim.pos = (Vector2)game.transform.position + new Vector2(Mathf.Lerp(range.min, range.max, e / duration), y);
				yield return null;
			}
			sim.pos = (Vector2)game.transform.position + new Vector2(range.max, y);
			yield return null;
			sim.down = false; yield return new WaitForSeconds(0.3f);
			Debug.Log("[WAVE] T3 card result=" + game.StatusText.text);
			yield return new WaitForSeconds(2f);
		}
	}
}
#endif
