using System.Collections.Generic;
using UnityEngine;

public class TimeOutPopupHandler : MonoBehaviour
{
	public UiElement BackButton;

	public List<UiElement> Buttons;

	public void Retry()
	{
		GameObject gameObject = base.gameObject;
		gameObject.SetActive(value: false);
	}

	public void PlayOffline()
	{
		EOSManager instance = DestroyableSingleton<EOSManager>.Instance;
		instance.PlayOffline();
		GameObject gameObject = base.gameObject;
		gameObject.SetActive(value: false);
	}

	public void Close()
	{
		GameObject gameObject = base.gameObject;
		gameObject.SetActive(value: false);
	}
}
