using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FriendsListConfirmMenu : MonoBehaviour
{
	private enum ConfirmationStatus
	{
		Waiting,
		Yes,
		No
	}

	public enum ActionType
	{
		Friend,
		Block,
		Lobby,
		Unfriend,
		Unblock
	}

	public TextMeshPro PopUpText;

	private ConfirmationStatus confirmStatus;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	public IEnumerator WaitForConfirm(Action cb, ActionType type, string recipientFriendCode)
	{
		switch (type)
		{
		case ActionType.Block:
			PopUpText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.BlockPlayerConfirm, recipientFriendCode);
			break;
		case ActionType.Friend:
			PopUpText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.AddFriendConfirm, recipientFriendCode);
			break;
		case ActionType.Lobby:
			PopUpText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.InviteToLobbyConfirm, recipientFriendCode);
			break;
		case ActionType.Unfriend:
			PopUpText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.UnfriendConfirm, recipientFriendCode);
			break;
		case ActionType.Unblock:
			PopUpText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.UnblockConfirm, recipientFriendCode);
			break;
		}
		gameObject.SetActive(value: true);
		confirmStatus = ConfirmationStatus.Waiting;
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultButtonSelected, ControllerSelectable);
		while (confirmStatus == ConfirmationStatus.Waiting)
		{
			yield return null;
		}
		if (confirmStatus == ConfirmationStatus.Yes)
		{
			cb();
		}
		gameObject.SetActive(value: false);
	}

	public void Confirm()
	{
		confirmStatus = ConfirmationStatus.Yes;
	}

	public void Cancel()
	{
		confirmStatus = ConfirmationStatus.No;
	}

	public virtual void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}
}
