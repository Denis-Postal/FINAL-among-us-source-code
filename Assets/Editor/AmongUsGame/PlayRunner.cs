#if UNITY_EDITOR
// Headless smoke-test driver. Usage (no -quit):
//   Unity -batchmode -nographics -projectPath AmongUsGame -executeMethod AmongUsGameTools.PlayRunner.Start -logFile x.log -- scene=MainMenu freeplay=Skeld seconds=40
// Enters play mode, loads the scene, optionally triggers Freeplay on a map, logs exceptions, exits with code 0/1.
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AmongUsGameTools
{
	[InitializeOnLoad]
	public static class PlayRunner
	{
		const string Key = "AmongUsGame.PlayRunner";
		static PlayRunner()
		{
			if (EditorPrefs.GetBool(Key, false) && !EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
			{
				EditorApplication.delayCall += () => { EditorApplication.isPlaying = true; };
			}
		}
		public static void Start()
		{
			var args = Environment.GetCommandLineArgs();
			var kv = new Dictionary<string, string>();
			foreach (var a in args) { int i = a.IndexOf('='); if (i > 0 && !a.StartsWith("-")) kv[a.Substring(0, i)] = a.Substring(i + 1); }
			EditorPrefs.SetBool(Key, true);
			EditorPrefs.SetString(Key + ".scene", kv.TryGetValue("scene", out var s) ? s : "MainMenu");
			EditorPrefs.SetString(Key + ".freeplay", kv.TryGetValue("freeplay", out var f) ? f : "");
			EditorPrefs.SetString(Key + ".seconds", kv.TryGetValue("seconds", out var sec) ? sec : "30");
			string sp = "Assets/" + EditorPrefs.GetString(Key + ".scene") + ".unity";
			if (!System.IO.File.Exists(sp)) sp = "Assets/EndGame/" + EditorPrefs.GetString(Key + ".scene") + ".unity";
			UnityEditor.SceneManagement.EditorSceneManager.OpenScene(sp);
			EditorApplication.isPlaying = true;
		}
	}
}
#endif
