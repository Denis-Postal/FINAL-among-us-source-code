using System.Collections.Generic;
using UnityEngine;

public class ControllerDisconnectHandler : MonoBehaviour
{
	private bool isConnected = true;

	public GameObject ContinueBackground;

	public GameObject ContinueText;

	public GameObject obj;

	public UiElement ContinueButton;

	private void Update()
	{
	}

	public void Close()
	{
		Debug.LogError("Controller reconnected");
		isConnected = true;
		if ((bool)ContinueBackground)
		{
			ContinueBackground.SetActive(value: false);
		}
		if ((bool)ContinueText)
		{
			ContinueText.SetActive(value: false);
		}
		if ((bool)obj)
		{
			obj.SetActive(value: false);
		}
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	private void OnStateChange(uint index, bool newIsConnected)
	{
		if (!newIsConnected && isConnected)
		{
			Debug.LogError("Controller disconnected");
			isConnected = false;
			if ((bool)ContinueBackground)
			{
				ContinueBackground.SetActive(value: true);
			}
			if ((bool)ContinueText)
			{
				ContinueText.SetActive(value: true);
			}
			if ((bool)obj)
			{
				obj.SetActive(value: true);
			}
			ControllerManager.Instance.OpenOverlayMenu(name, ContinueButton, ContinueButton);
		}
	}
}
