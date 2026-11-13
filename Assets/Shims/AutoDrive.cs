// Scripted driver for player builds, used to test two game instances on one machine (host + joiner) without a human.
// Only active when the command line contains  -ag_drive "<sec>:<cmd>;<sec>:<cmd>;..."  (seconds since start, commands below).
// Optional: -ag_shotdir <dir> (default: next to the log) for shot=NAME.  Normal play is unaffected.
//   mclick=<PassiveButton name>   click the first active button with that name (hover, down, up like a mouse)
//   joinfirst                     click the first JoinGameButton in the local game list
//   shot=<name>                   screenshot to <shotdir>/<name>.png
//   state                         log scene / client / player list
//   fake                          host: spawn one fake test character (like the debug "Create character")
//   press=<Action> hold=<Action> release=<Action>   Rewired action names (RewiredConsts.Action)
//   tp=<x>;<y>                    snap the local player
//   log=<text>   quit
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AmongUsGameTools
{
	public class AutoDrive : MonoBehaviour
	{
		private struct Step { public float time; public string cmd; public bool done; }

		private readonly List<Step> steps = new List<Step>();

		private string shotDir;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Boot()
		{
			string[] args = Environment.GetCommandLineArgs();
			string script = null, shots = null;
			for (int i = 0; i < args.Length - 1; i++)
			{
				if (args[i] == "-ag_drive") script = args[i + 1];
				if (args[i] == "-ag_shotdir") shots = args[i + 1];
			}
			if (string.IsNullOrEmpty(script)) return;
			GameObject go = new GameObject("AutoDrive");
			DontDestroyOnLoad(go);
			AutoDrive d = go.AddComponent<AutoDrive>();
			d.shotDir = shots ?? Application.persistentDataPath;
			foreach (string part in script.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
			{
				int c = part.IndexOf(':');
				if (c <= 0) continue;
				// "tp=1;2" contains ';' - written as "tp=1,2" in scripts
				d.steps.Add(new Step { time = float.Parse(part.Substring(0, c), System.Globalization.CultureInfo.InvariantCulture), cmd = part.Substring(c + 1) });
			}
			Application.runInBackground = true;
			Debug.Log("[AutoDrive] script loaded: " + d.steps.Count + " steps, shots -> " + d.shotDir);
		}

		private void Update()
		{
			for (int i = 0; i < steps.Count; i++)
			{
				Step s = steps[i];
				if (s.done || Time.realtimeSinceStartup < s.time) continue;
				s.done = true;
				steps[i] = s;
				try { Run(s.cmd); }
				catch (Exception e) { Debug.Log("[AutoDrive] FAIL " + s.cmd + " : " + e.GetType().Name + " " + e.Message); }
			}
		}

		private static bool Click(PassiveButton b)
		{
			if (b == null) return false;
			b.ReceiveMouseOver();
			if (b.HandleDownClickGraphic) b.ReceiveClickDownGraphic();
			if (b.HandleDown) b.ReceiveClickDown();
			if (b.HandleUp) b.ReceiveClickUp();
			if (b.HandleUpClickGraphic) b.ReceiveClickUpGraphic();
			return true;
		}

		private static int ActionId(string name)
		{
			return (int)typeof(RewiredConsts.Action).GetField(name).GetRawConstantValue();
		}

		private void Run(string cmd)
		{
			Debug.Log("[AutoDrive] t=" + Time.realtimeSinceStartup.ToString("F1") + " " + cmd);
			string arg = "";
			int eq = cmd.IndexOf('=');
			if (eq > 0) { arg = cmd.Substring(eq + 1); cmd = cmd.Substring(0, eq); }
			switch (cmd)
			{
			case "mclick":
			{
				PassiveButton found = FindObjectsByType<PassiveButton>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).FirstOrDefault(pb => pb.name == arg && pb.isActiveAndEnabled);
				Debug.Log("[AutoDrive] mclick " + arg + " -> " + (found ? "ok" : "NOT FOUND"));
				Click(found);
				break;
			}
			case "joinfirst":
			{
				var addrField = typeof(JoinGameButton).GetField("netAddress", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
				JoinGameButton jb = FindObjectsByType<JoinGameButton>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).FirstOrDefault(j => addrField == null || !string.IsNullOrEmpty((string)addrField.GetValue(j)));
				Debug.Log("[AutoDrive] joinfirst -> " + (jb ? jb.name : "NO JOIN BUTTON (no games discovered)"));
				if (jb) Click(jb.GetComponent<PassiveButton>());
				break;
			}
			case "shot":
				System.IO.Directory.CreateDirectory(shotDir);
				ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(shotDir, arg + ".png"));
				break;
			case "fake":
				AmongUsClient.Instance.StartCoroutine(FakePlayerFactory.CoCreate());
				break;
			case "state":
				LogState();
				break;
			case "press":
				Rewired.KeyMap.Pressed[ActionId(arg)] = Time.frameCount;
				break;
			case "hold":
				Rewired.KeyMap.TestHeld.Add(ActionId(arg));
				break;
			case "release":
				Rewired.KeyMap.TestHeld.Remove(ActionId(arg));
				break;
			case "tp":
			{
				string[] xy = arg.Split(',');
				PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(new Vector2(float.Parse(xy[0], System.Globalization.CultureInfo.InvariantCulture), float.Parse(xy[1], System.Globalization.CultureInfo.InvariantCulture)));
				break;
			}
			case "log":
				Debug.Log("[AutoDrive] LOG " + arg);
				break;
			case "quit":
				Application.Quit();
				break;
			default:
				Debug.Log("[AutoDrive] unknown command " + cmd);
				break;
			}
		}

		private static void LogState()
		{
			AmongUsClient c = AmongUsClient.Instance;
			string line = "[AutoDrive] STATE scene=" + SceneManager.GetActiveScene().name;
			if (c != null)
			{
				line += " mode=" + c.NetworkMode + " client=" + c.ClientId + " host=" + c.HostId + " amHost=" + c.AmHost + " gameId=" + c.GameId + " state=" + c.GameState;
			}
			Debug.Log(line);
			if (GameData.Instance != null)
			{
				foreach (NetworkedPlayerInfo p in GameData.Instance.AllPlayers)
				{
					Debug.Log("[AutoDrive] PLAYER id=" + p.PlayerId + " client=" + p.ClientId + " name=" + p.PlayerName + " role=" + p.RoleType + " dead=" + p.IsDead + " disc=" + p.Disconnected + " tasks=" + (p.Tasks != null ? p.Tasks.Count : -1) + (p.Object ? " pos=" + p.Object.transform.position.ToString("F2") + " own=" + p.Object.AmOwner : " (no object)"));
				}
			}
		}
	}
}
