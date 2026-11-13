using Innersloth.DebugTool;
using UnityEngine;

public class DebugAnalytics : DestroyableSingleton<DebugAnalytics>
{
	public static bool AnalyticsEnabled;

	private Texture2D consoleBackground;

	private GUIStyle debugStyle;

	private const string analyticsText = "Analytics Enabled";

	// TODO(verify): initialize the native offline DummyAnalytics before first synchronous
	// completion; this singleton can be created during CompleteTask, before Start runs.
	public IAnalyticsReporter Analytics { get; private set; } = new DummyAnalytics();

	private unsafe void OnGUI()
	{
		if (AnalyticsEnabled)
		{
			GUI.Label(new Rect(10f, 10f, 150f, 25f), "Analytics Enabled", debugStyle);
		}
	}

	private unsafe void Start()
	{
		CreateAnalyticsReporter();
		DebugTool.RegisterCommandHandler(this);
		consoleBackground = new Texture2D(1, 1, TextureFormat.RGBAFloat, mipChain: false);
		consoleBackground.SetPixel(0, 0, new Color(1f, 1f, 1f, 0.25f));
		consoleBackground.Apply();
		debugStyle = new GUIStyle(GUIStyle.none);
		debugStyle.fontSize = 16;
		debugStyle.normal.textColor = Color.red;
		debugStyle.normal.background = consoleBackground;
	}

	private new void OnDestroy()
	{
		DebugTool.UnregisterCommandHandler(this);
		base.OnDestroy();
	}

	public static void ToggleAnalytics()
	{
		DebugTool.LogCommandSuccess($"Toggled Playtest Analytics: 'Enabled = {AnalyticsEnabled}'");
	}

	private void CreateAnalyticsReporter()
	{
		_ = AnalyticsEnabled;
		Analytics = new DummyAnalytics();
	}
}
