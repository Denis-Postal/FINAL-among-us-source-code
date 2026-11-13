using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Opt-in local executable smoke capture; inert unless explicitly launched with this flag.
public class OfflinePlayerCapture : MonoBehaviour
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Begin()
	{
		if (Array.IndexOf(Environment.GetCommandLineArgs(), "--codex-smoke") < 0) return;
		var capture = new GameObject("OfflinePlayerCapture");
		DontDestroyOnLoad(capture);
		capture.AddComponent<OfflinePlayerCapture>();
	}

	private IEnumerator Start()
	{
		string folder = "C:/spacemafia-game/scratch/shots";
		Directory.CreateDirectory(folder);
		float[] times = { 2f, 8f, 60f };
		float start = Time.realtimeSinceStartup;
		for (int i = 0; i < times.Length; i++)
		{
			while (Time.realtimeSinceStartup - start < times[i]) yield return null;
			yield return new WaitForEndOfFrame();
			Debug.Log("[OfflinePlayerCapture] scene=" + SceneManager.GetActiveScene().name + " elapsed=" + (Time.realtimeSinceStartup - start));
			ScreenCapture.CaptureScreenshot(folder + "/p3_player_" + i + ".png");
		}
		yield return new WaitForSecondsRealtime(1f);
		Application.Quit();
	}
}
