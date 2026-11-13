#if UNITY_EDITOR
using System;
using System.Collections;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace AmongUsGameTools
{
	public static class FreeplayWaveCapture
	{
		public static bool Run(string action)
		{
			if(MonoUICapture.Run(action)) return true;
			if(MonoRolesCapture.Run(action)) return true;
			if(Tasks3Capture.Run(action)) return true;
			if(Fix4bCapture.Run(action)) return true;
			if(Fix4aCapture.Run(action)) return true;
			if (Meet3Capture.Run(action)) return true;
			if(action == "laptop" || action == "emergency")
			{
				HudManager.Instance.StartCoroutine(OpenSystem(action == "laptop"));
				return true;
			}
			if(action == "meetingSkip")
			{
				var meeting = MeetingHud.Instance;
				if(meeting) typeof(MeetingHud).GetMethod("ForceSkipAll", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(meeting, null);
				return true;
			}
			if(action == "info") { MatchInfoGuide.Instance.Open(); return true; }
			if(action == "infoclose") { MatchInfoGuide.Instance.Close(); return true; }
			if(action.StartsWith("infotab=")) { MatchInfoGuide.Instance.SetActiveTab(int.Parse(action.Substring(8))); return true; }
			if(action == "debugcheck")
			{
				int before = GameData.Instance.PlayerCount;
				var command = Innersloth.DebugTool.DebugTool.Commands.First(c => c.CommandPath == "Freeplay/Create character");
				command.ActionInfo.Invoke(command.Target, new object[0]);
				Debug.Log("[WAVE] debug create count=" + before + "->" + GameData.Instance.PlayerCount + " commands=" + Innersloth.DebugTool.DebugTool.Commands.Count);
				return true;
			}
			if (action.StartsWith("tour="))
			{
				HudManager.Instance.StartCoroutine(Tour(action.Substring(5).Split(';')));
				return true;
			}
			if (action != "cardswipe") return false;
			var game = UnityEngine.Object.FindFirstObjectByType<CardSlideGame>();
			if (!game) { Debug.Log("[WAVE] cardswipe: no CardSlideGame"); return true; }
			game.StartCoroutine(Swipe(game));
			return true;
		}
		static IEnumerator OpenSystem(bool laptop)
		{
			if(Minigame.Instance) Minigame.Instance.Close();
			yield return new WaitForSeconds(.6f);
			var console = UnityEngine.Object.FindObjectsByType<SystemConsole>(FindObjectsSortMode.None).FirstOrDefault(c => laptop ? c.MinigamePrefab is TaskAdderGame : c.MinigamePrefab is EmergencyMinigame);
			if(!console) { Debug.Log("[WAVE] system console missing laptop="+laptop); yield break; }
			PlayerControl.LocalPlayer.NetTransform.RpcSnapTo((Vector2)console.transform.position + new Vector2(0,-.4f));
			yield return new WaitForSeconds(.5f);
			console.Use();
			yield return new WaitForSeconds(1);
			Debug.Log("[WAVE] system open " + (Minigame.Instance ? Minigame.Instance.GetType().Name : "none"));
			if(!laptop && Minigame.Instance is EmergencyMinigame emergency) emergency.CallMeeting();
		}
		static IEnumerator Tour(string[] types)
		{
			foreach (string name in types)
			{
				if (Minigame.Instance) Minigame.Instance.Close();
				yield return new WaitForSeconds(.5f);
				var type = (TaskTypes)Enum.Parse(typeof(TaskTypes), name);
				var task = PlayerControl.LocalPlayer.myTasks.OfType<NormalPlayerTask>().FirstOrDefault(t => t.TaskType == type && !t.IsComplete);
				if (!task)
				{
					var prefab = ShipStatus.Instance.CommonTasks.Concat(ShipStatus.Instance.LongTasks).Concat(ShipStatus.Instance.ShortTasks).FirstOrDefault(t => t.TaskType == type);
					if (!prefab) { Debug.Log("[WAVE] tour missing prefab " + name); continue; }
					TaskAddButton.AddTaskToPlayer(prefab);
					task = PlayerControl.LocalPlayer.myTasks.OfType<NormalPlayerTask>().Last(t => t.TaskType == type);
				}
				var findTask = typeof(Console).GetMethod("FindTask", BindingFlags.Instance | BindingFlags.NonPublic);
				var console = UnityEngine.Object.FindObjectsByType<Console>(FindObjectsSortMode.None).FirstOrDefault(c => findTask.Invoke(c, new object[] { PlayerControl.LocalPlayer }) == (object)task);
				if (!console) { Debug.Log("[WAVE] tour missing console " + name); continue; }
				PlayerControl.LocalPlayer.NetTransform.RpcSnapTo((Vector2)console.transform.position + new Vector2(0,-.4f));
				yield return new WaitForSeconds(.5f);
				try { console.Use(); } catch(Exception ex) { Debug.LogException(ex); }
				yield return new WaitForSeconds(1f);
				var game = Minigame.Instance;
				Debug.Log("[WAVE] tour open " + name + " game=" + (game ? game.GetType().Name : "none") + " step=" + task.taskStep);
				ScreenCapture.CaptureScreenshot("C:/spacemafia-game/scratch/shots/w254_" + name + ".png");
				if (!game) continue;
				IEnumerator interaction = Interact(game, task);
				// Advance the top-level harness with a bounded exception boundary; task coroutines remain the production paths.
				while(true)
				{
					bool next;
					try { next = interaction.MoveNext(); } catch(Exception ex) { Debug.LogException(ex); break; }
					if (!next) break;
					yield return interaction.Current;
				}
				Debug.Log("[WAVE] tour result " + name + " step=" + task.taskStep + "/" + task.MaxStep + " count=" + GameData.Instance.CompletedTasks + "/" + GameData.Instance.TotalTasks);
				if (game) game.Close();
				yield return new WaitForSeconds(.5f);
				Debug.Log("[WAVE] tour closed " + name + " canMove=" + PlayerControl.LocalPlayer.CanMove);
			}
			Debug.Log("[WAVE] tour FINISHED");
		}
		static IEnumerator Interact(Minigame game, NormalPlayerTask task)
		{
			if(game is CardSlideGame card) { yield return Swipe(card); yield return new WaitForSeconds(2); }
			else if(game is ShieldMinigame shield)
			{
				for(int i=0;i<shield.Shields.Length;i++) if((task.Data[0] & (1<<i)) == 0) { shield.ToggleShield(i); yield return new WaitForSeconds(.15f); }
				yield return new WaitForSeconds(2);
			}
			else if(game is UploadDataGame upload) { upload.Click(); yield return new WaitForSeconds(11); }
			else if(game is RefuelStage fuel) { fuel.Refuel(); yield return new WaitForSeconds(fuel.RefuelDuration+1); fuel.Refuel(); }
			else if(game is SweepMinigame sweep)
			{
				var calc = typeof(SweepMinigame).GetMethod("CalcXPerc", BindingFlags.Instance|BindingFlags.NonPublic);
				for(int i=0;i<sweep.Spinners.Length;i++)
				{
					float end = Time.time + 12;
					while(Time.time < end && (float)calc.Invoke(sweep,null) >= 10) yield return null;
					sweep.HitButton(i); yield return new WaitForSeconds(.15f);
				}
				yield return new WaitForSeconds(2);
			}
			else if(game is MedScanMinigame scan) { yield return new WaitForSeconds(scan.ScanDuration+5); }
			else if(game is SampleMinigame sample) { sample.NextStep(); yield return new WaitForSeconds(3); }
			else if(game is WeaponsMinigame weapons)
			{
				float end=Time.time+20;
				while(weapons && !task.IsComplete && Time.time<end)
				{
					var asteroid=weapons.asteroidPool.activeChildren.OfType<Asteroid>().FirstOrDefault(a=>a && a.enabled);
					if(asteroid) asteroid.GetComponent<ButtonBehavior>().OnClick.Invoke();
					yield return new WaitForSeconds(.35f);
				}
			}
			else { yield return new WaitForSeconds(.5f); }
		}
		static IEnumerator Swipe(CardSlideGame game)
		{
			const BindingFlags fields = BindingFlags.Instance | BindingFlags.NonPublic;
			var controller = (Controller)typeof(CardSlideGame).GetField("myController", fields).GetValue(game);
			var range = (FloatRange)typeof(CardSlideGame).GetField("XRange", fields).GetValue(game);
			var touch = controller.Touches[0];
			Vector2 position = game.col.bounds.center;
			bool down = false, previous = false;
			Controller.CaptureUpdate = input =>
			{
				if (input != controller) return false;
				touch.Position = position;
				touch.TouchStart = down && !previous;
				touch.TouchEnd = !down && previous;
				touch.IsDown = down;
				touch.active = true;
				if (touch.TouchStart) touch.DownAt = position;
				previous = down;
				return true;
			};
			try
			{
				down = true; yield return new WaitForSeconds(0.1f);
				down = false; yield return new WaitForSeconds(0.8f);
				Debug.Log("[WAVE] card inserted state=" + typeof(CardSlideGame).GetField("State", fields).GetValue(game));
				position = game.col.bounds.center;
				down = true; yield return new WaitForSeconds(0.05f);
				float duration = (game.AcceptedTime.min + game.AcceptedTime.max) / 2f;
				for (float elapsed = 0f; elapsed < duration; elapsed += Time.deltaTime)
				{
					position = (Vector2)game.transform.position + new Vector2(Mathf.Lerp(range.min, range.max, elapsed / duration), game.col.transform.localPosition.y);
					yield return null;
				}
				position = (Vector2)game.transform.position + new Vector2(range.max, game.col.transform.localPosition.y);
				yield return null;
				down = false; yield return new WaitForSeconds(0.1f);
				Debug.Log("[WAVE] card result=" + game.StatusText.text + " dragTime=" + game.dragTime + " tasks=" + GameData.Instance.CompletedTasks + "/" + GameData.Instance.TotalTasks);
			}
			finally { Controller.CaptureUpdate = null; }
		}
	}
}
#endif
