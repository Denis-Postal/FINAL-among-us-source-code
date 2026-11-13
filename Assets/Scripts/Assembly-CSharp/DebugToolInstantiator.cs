using Innersloth.DebugTool;
using UnityEngine;

public class DebugToolInstantiator : MonoBehaviour
{
	[SerializeField]
	private GameObject debugOverlay;

	[SerializeField]
	private GameObject memoryStatsView;

	private static DebugToolOverlay overlayInstance;

	// TODO(verify): the native Awake body is lost (decomp9 kept only the fields). The overlay prefab is spawned once,
	// kept across scenes and opened with the BackQuote key. EnsureOverlay is also called from Freeplay/HUD start so the
	// menu is reachable when the splash scene (the only scene holding this component) was skipped.
	private void Awake()
	{
		EnsureMemoryStats(memoryStatsView);
		EnsureOverlay(debugOverlay);
	}

	public static DebugToolOverlay EnsureOverlay(GameObject prefab = null)
	{
		if (overlayInstance != null)
		{
			return overlayInstance;
		}
		if (prefab == null)
		{
			prefab = Resources.Load<GameObject>("DebugToolOverlay");
		}
		if (prefab == null)
		{
			Debug.LogWarning("[DebugTool] overlay prefab not found");
			return null;
		}
		EnsureMemoryStats();
		GameObject go = Object.Instantiate(prefab);
		go.name = "DebugToolOverlay";
		Object.DontDestroyOnLoad(go);
		// fix4a: the overlay is plain UGUI (Button/ScrollRect/Canvas+GraphicRaycaster) but the game has no EventSystem (it uses its own
		// PassiveButton/Controller), so mouse clicks never reached it. Add one (old Input module; activeInputHandler=0) as a child so it persists with the overlay.
		bool haveEventSystem = false;
		foreach (UnityEngine.EventSystems.EventSystem existing in Object.FindObjectsByType<UnityEngine.EventSystems.EventSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None))
		{
			if (existing.isActiveAndEnabled && existing.GetComponent<UnityEngine.EventSystems.BaseInputModule>() != null)
			{
				haveEventSystem = true;
			}
		}
		if (!haveEventSystem)
		{
			GameObject es = new GameObject("DebugToolEventSystem");
			es.transform.SetParent(go.transform, false);
			es.AddComponent<UnityEngine.EventSystems.EventSystem>();
			es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
		}
		overlayInstance = go.GetComponent<DebugToolOverlay>();
		overlayInstance.RegisterInputProvider(new DebugToolKeyboardInputProvider());
		overlayInstance.RegisterKeyboardInputProvider(new DebugToolNullKeyboardInputProvider());
		overlayInstance.Initialize();
		Innersloth.DebugTool.DebugTool.RegisterCommandHandler(typeof(LocalFreeplayDebugCommands));
		Innersloth.DebugTool.DebugTool.RegisterCommandHandler(typeof(GameDebugCommands));
		Innersloth.DebugTool.DebugTool.RegisterCommandHandler(typeof(LobbyDebugCommands));
		Innersloth.DebugTool.DebugTool.RegisterCommandHandler(typeof(TutorialDebugCommands));
		Innersloth.DebugTool.DebugTool.RegisterCommandHandler(typeof(CosmeticDebugCommands));
		Debug.Log("[DebugTool] overlay ready (BackQuote / F1 / F2 / F3 toggles)");
		return overlayInstance;
	}

	public static void SetOverlayVisible(bool visible)
	{
		DebugToolOverlay overlay = EnsureOverlay();
		if (overlay != null)
		{
			overlay.ToggleUIVisible(visible);
		}
	}
    private static void EnsureMemoryStats(GameObject prefab = null)
    {
        DebugToolMemoryStatsView existing = Object.FindFirstObjectByType<DebugToolMemoryStatsView>(FindObjectsInactive.Include);
        if (existing != null) { existing.gameObject.SetActive(true); return; }
        // TODO(verify): local bootstrap fallback uses the exported prefab's GUID when Splash was skipped.
        if (prefab == null) prefab = UnityEngine.AddressableAssets.AssetGuidRegistryAccess.Resolve("a5d2879b56c594544a03faedfb19f499") as GameObject;
        if (prefab == null) return;
        GameObject stats = Object.Instantiate(prefab);
        // TODO(verify): exported debug prefab root is inactive; native page lookup requires an active view.
        stats.SetActive(true);
        Object.DontDestroyOnLoad(stats);
        DebugToolMemoryStatsView view = stats.GetComponent<DebugToolMemoryStatsView>();
        if (view != null) view.ToggleVisibility(false);
    }
}
