// Runtime half of the headless smoke test (see unity_editor/PlayRunner.cs). Inactive unless EditorPrefs flag set.
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Text;
using UnityEditor;
using UnityEngine;

public class AutoTestDriver : MonoBehaviour
{
	const string Key = "AmongUsGame.PlayRunner";
	int errors;
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	static void Boot()
	{
		if (!EditorPrefs.GetBool(Key, false)) return;
		var go = new GameObject("AutoTestDriver");
		DontDestroyOnLoad(go);
		go.AddComponent<AutoTestDriver>();
	}
	void OnEnable() { Application.logMessageReceived += OnLog; }
	void OnDisable() { Application.logMessageReceived -= OnLog; }
	void OnLog(string msg, string stack, LogType t)
	{
		if (t == LogType.Exception || t == LogType.Error) { errors++; }
	}
	IEnumerator Start()
	{
		float secs = float.Parse(EditorPrefs.GetString(Key + ".seconds", "30"));
		string fp = EditorPrefs.GetString(Key + ".freeplay", "");
		Debug.Log("[AutoTest] begin scene=" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " freeplay=" + fp);
		yield return new WaitForSeconds(5f);
		if (fp != "")
		{
			Debug.Log("[AutoTest] requesting freeplay " + fp);
			try { AutoTestHooks.StartFreeplay(fp); } catch (Exception e) { Debug.LogError("[AutoTest] StartFreeplay threw " + e); }
		}
		yield return new WaitForSeconds(secs);
		Debug.Log("[AutoTest] done errors=" + errors + " scene=" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		EditorPrefs.SetBool(Key, false);
		EditorApplication.Exit(errors == 0 ? 0 : 1);
	}
}
public static class AutoTestHooks
{
	// TODO(verify): invokes the same entry the Freeplay popover uses (HostLocalGameButton.OnClick with TutorialMapId = map).
	public static void StartFreeplay(string mapName)
	{
		int map = (int)(MapNames)Enum.Parse(typeof(MapNames), mapName);
		if (GameOptionsManager.Instance == null) { new GameOptionsManager(new InnerNet.UnityLogger()); GameOptionsManager.Instance.Initialize(); } // test shim: the real creator lives in SplashIntro
		AmongUsClient.Instance.TutorialMapId = map;
		var pop = UnityEngine.Object.FindAnyObjectByType<FreeplayPopover>(FindObjectsInactive.Include); if (pop != null) pop.Show(); // popover hosts the coroutine runner
		var btn = UnityEngine.Object.FindAnyObjectByType<HostLocalGameButton>(FindObjectsInactive.Include);
		if (btn == null) { Debug.LogError("[AutoTest] no HostLocalGameButton in scene"); return; }
		btn.NetworkMode = NetworkModes.FreePlay;
		btn.OnClick();
	}
}
#endif
