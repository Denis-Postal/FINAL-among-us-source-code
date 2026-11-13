using System.Linq;
using AmongUs.Data;
using TMPro;
using UnityEngine;

public class ResolutionSlider : MonoBehaviour
{
	private int targetIdx;

	private Resolution targetResolution;

	private bool targetFullscreen;

	private Resolution[] allResolutions;

	public SlideBar slider;

	public ToggleButtonBehaviour Fullscreen;

	public ToggleButtonBehaviour VSync;

	public TextMeshPro Display;

	public ToggleButtonBehaviour Screenshake;

	public void OnEnable()
	{
		allResolutions = Screen.resolutions.Where((Resolution r) => r.height > 480).ToArray();
		if (Screen.fullScreen)
		{
			targetResolution = Screen.currentResolution;
		}
		else
		{
			targetResolution = new Resolution
			{
				height = Screen.height,
				width = Screen.width
			};
		}
		targetFullscreen = Screen.fullScreen;
		targetIdx = allResolutions.IndexOf((Resolution e) => e.width == targetResolution.width && e.height == targetResolution.height);
		slider.Value = (float)targetIdx / ((float)allResolutions.Length - 1f);
		Display.text = $"{targetResolution.width}x{targetResolution.height}";
		Fullscreen.UpdateText(targetFullscreen);
		VSync.UpdateText(DataManager.Settings.Video.VSync);
		Screenshake.UpdateText(DataManager.Settings.Gameplay.ScreenShake);
	}

	public void ToggleScreenshake()
	{
		DataManager.Settings.Gameplay.ScreenShake = !DataManager.Settings.Gameplay.ScreenShake;
		DataManager.Settings.Save();
		Screenshake.UpdateText(DataManager.Settings.Gameplay.ScreenShake);
	}

	public void ToggleVSync()
	{
		DataManager.Settings.Video.VSync = !DataManager.Settings.Video.VSync;
		if (DataManager.Settings.Video.VSync)
		{
			QualitySettings.vSyncCount = 1;
		}
		else
		{
			QualitySettings.vSyncCount = 0;
		}
		VSync.UpdateText(DataManager.Settings.Video.VSync);
	}

	public void ToggleFullscreen()
	{
		targetFullscreen = !targetFullscreen;
		Fullscreen.UpdateText(targetFullscreen);
	}

	public void OnResChange()
	{
		int num = Mathf.RoundToInt((float)(allResolutions.Length - 1) * slider.Value);
		if (num != targetIdx)
		{
			targetIdx = num;
			targetResolution = allResolutions[num];
			Display.text = $"{targetResolution.width}x{targetResolution.height}";
		}
		slider.Value = (float)targetIdx / ((float)allResolutions.Length - 1f);
	}

	public void SaveChange()
	{
		ResolutionManager.SetResolution(targetResolution.width, targetResolution.height, targetFullscreen);
	}
}
