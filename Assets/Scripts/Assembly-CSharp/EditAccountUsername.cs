using System;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Assets.InnerNet;
using TMPro;
using UnityEngine;

public class EditAccountUsername : MonoBehaviour
{
	public TextMeshPro UsernameText;

	public TextMeshPro SuccessText;

	public GameObject SuccessMessage;

	public GameObject PopUp;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	public void OnEnable()
	{
		string menuName = name;
		List<UiElement> selectableElements = default;
		bool gridNav = default;
		ControllerManager.Instance.OpenOverlayMenu(menuName, null, DefaultButtonSelected, selectableElements, gridNav);
	}

	public virtual void OnDisable()
	{
		string menuName = name;
		ControllerManager.Instance.CloseOverlayMenu(menuName);
	}

	public void SaveUsername()
	{
		string text = UsernameText.text;
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		FriendsListManager instance = DestroyableSingleton<FriendsListManager>.Instance;
		string text2 = UsernameText.text;
		Action<ResponseState, Response<ResponseFriendCode>> resultCallback = (ResponseState cb, Response<ResponseFriendCode> response) =>
		{
			if (cb == ResponseState.Success)
			{
				PopUp.SetActive(value: false);
				SuccessMessage.SetActive((byte)cb != 0);
				Response<ResponseFriendCode>.ResponseData data = response.Data;
				ResponseFriendCode attributes = data.Attributes;
				string friendCode = attributes.Username + "#" + attributes.Discriminator;
				EOSManager instance2 = DestroyableSingleton<EOSManager>.Instance;
				instance2.friendCode = friendCode;
				SettingsData settings = DataManager.Settings;
				settings.multiplayer.AllowFriendInvites = true;
				SettingsData settings2 = DataManager.Settings;
				settings2.Save();
			}
		};
		instance.SetFriendCode(text2, resultCallback);
	}

	public void RandomizeName()
	{
		AccountManager instance = DestroyableSingleton<AccountManager>.Instance;
		string randomName = instance.GetRandomName();
		UsernameText.SetText(randomName);
	}

	public void Close()
	{
		PopUp.SetActive(value: true);
		SuccessMessage.SetActive(value: false);
		GameObject gameObject = base.gameObject;
		gameObject.SetActive(value: false);
	}

	private void _003CSaveUsername_003Eb__8_0(ResponseState cb, Response<ResponseFriendCode> response)
	{
		if (cb == ResponseState.Success)
		{
			PopUp.SetActive(value: false);
			SuccessMessage.SetActive((byte)cb != 0);
			Response<ResponseFriendCode>.ResponseData data = response.Data;
			ResponseFriendCode attributes = data.Attributes;
			string friendCode = attributes.Username + "#" + attributes.Discriminator;
			EOSManager instance = DestroyableSingleton<EOSManager>.Instance;
			instance.friendCode = friendCode;
			SettingsData settings = DataManager.Settings;
			settings.multiplayer.AllowFriendInvites = true;
			SettingsData settings2 = DataManager.Settings;
			settings2.Save();
		}
	}
}
