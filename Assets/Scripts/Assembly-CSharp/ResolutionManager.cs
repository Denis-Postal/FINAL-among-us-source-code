using UnityEngine;

public static class ResolutionManager
{
	public delegate void ResolutionChangedHandler(float aspectRatio, int width, int height, bool fullscreen);

	public static event ResolutionChangedHandler ResolutionChanged;

	public static void SetResolution(int width, int height, bool fullscreen)
	{
		Screen.SetResolution(width, height, fullscreen);
		DestroyableSingleton<CoroutineHelper>.Instance.DelayByFrame(() =>
		{
			ResolutionChanged?.Invoke((float)width / (float)height, width, height, fullscreen);
		});
	}

	public static void ToggleFullscreen()
	{
		bool flag = !Screen.fullScreen;
		int width;
		int height;
		if (flag)
		{
			Resolution[] resolutions = Screen.resolutions;
			Resolution resolution = resolutions[0];
			for (int i = 0; i < resolutions.Length; i++)
			{
				if (resolution.height < resolutions[i].height)
				{
					resolution = resolutions[i];
				}
			}
			width = resolution.width;
			height = resolution.height;
		}
		else
		{
			width = 720;
			height = 576;
		}
		SetResolution(width, height, flag);
	}
}
