using UnityEngine;

public class LoadingBarManager : DestroyableSingleton<LoadingBarManager>
{
	[SerializeField]
	private AmongUsLoadingBar loadingBar;

	public void ToggleLoadingBar(bool on)
	{
		// null guard: the loading bar prefab is only present in some scenes (Freeplay/offline path has none)
		if (loadingBar == null)
		{
			return;
		}
		GameObject gameObject = loadingBar.gameObject;
		gameObject.SetActive(on);
	}

	public void SetLoadingPercent(float percent, StringNames loadText)
	{
		if (loadingBar != null)
		{
			loadingBar.SetLoadingPercent(percent, loadText);
		}
	}
}
