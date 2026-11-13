using System.Collections;
using System.Collections.Generic;
using InnerNet;
using UnityEngine;

public class LobbyInfoPane : DestroyableSingleton<LobbyInfoPane>
{
	[SerializeField]
	private SpriteRenderer InfoPaneBackground;

	[SerializeField]
	private Sprite InfoPaneInactive;

	[SerializeField]
	private Sprite InfoPaneActive;

	[SerializeField]
	private UiElement EditButton;

	[SerializeField]
	private UiElement HostViewButton;

	[SerializeField]
	private UiElement ClientViewButton;

	[SerializeField]
	private UiElement CopyCodeButton;

	[SerializeField]
	private UiElement ActivateButton;

	[SerializeField]
	private UiElement BackButton;

	[SerializeField]
	private List<UiElement> ControllerSelectable;

	[SerializeField]
	private UiElement HostPrivateButton;

	[SerializeField]
	private UiElement HostPublicButton;

	[SerializeField]
	private LobbyViewSettingsPane LobbyViewSettingsPane;

	[SerializeField]
	private GameObject CopyCodeText;

	[SerializeField]
	private AudioClip CopyCodeSound;

	private UiElement DefaultButtonSelected;

	private IEnumerator copyGameCodeCoroutine;

	private bool IsPaneActive;

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
		DisableCopyCodeText();
	}

	private void Update()
	{
		if (IsPaneActive && Controller.currentTouchType != Controller.TouchType.Joystick)
		{
			DeactivatePane();
		}
	}

	public void ActivatePane()
	{
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			PlayerControl.LocalPlayer.NetTransform.Halt();
			InfoPaneBackground.sprite = InfoPaneActive;
			ControllerSelectable.Clear();
			ControllerSelectable.Add(CopyCodeButton);
			if (AmongUsClient.Instance.AmHost)
			{
				ControllerSelectable.Add(HostViewButton);
				ControllerSelectable.Add(EditButton);
				ControllerSelectable.Add(HostPrivateButton);
				ControllerSelectable.Add(HostPublicButton);
				DefaultButtonSelected = EditButton;
			}
			else
			{
				ControllerSelectable.Add(ClientViewButton);
				DefaultButtonSelected = ClientViewButton;
			}
			BackButton.gameObject.SetActive(value: true);
			ActivateButton.gameObject.SetActive(value: false);
			ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultButtonSelected, ControllerSelectable);
			IsPaneActive = true;
		}
	}

	public void DeactivatePane()
	{
		InfoPaneBackground.sprite = InfoPaneInactive;
		BackButton.gameObject.SetActive(value: false);
		ActivateButton.gameObject.SetActive(value: true);
		ControllerManager.Instance.CloseOverlayMenu(name);
		IsPaneActive = false;
	}

	public void RefreshPane()
	{
		if (LobbyViewSettingsPane.gameObject.activeSelf)
		{
			LobbyViewSettingsPane.RefreshTab();
		}
	}

	public void CopyGameCode()
	{
		GUIUtility.systemCopyBuffer = GameCode.IntToGameName(AmongUsClient.Instance.GameId);
		Debug.Log("[GameStartManager] Game name copied");
		SoundManager.Instance.PlaySoundImmediate(CopyCodeSound, loop: false);
		if (copyGameCodeCoroutine != null)
		{
			StopCoroutine(copyGameCodeCoroutine);
		}
		copyGameCodeCoroutine = CoCopyGameCode();
		StartCoroutine(copyGameCodeCoroutine);
	}

	private IEnumerator CoCopyGameCode()
	{
		CopyCodeText.SetActive(value: true);
		yield return new WaitForSeconds(2f);
		DisableCopyCodeText();
	}

	private void DisableCopyCodeText()
	{
		CopyCodeText.SetActive(value: false);
		copyGameCodeCoroutine = null;
	}
}
