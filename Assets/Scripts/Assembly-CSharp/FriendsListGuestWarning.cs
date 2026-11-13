using System;
using AmongUs.Data;
using AmongUs.Data.Player;
using Epic.OnlineServices.KWS;
using TMPro;
using UnityEngine;

public class FriendsListGuestWarning : MonoBehaviour
{
	public TextMeshPro Text;

	public GameObject EmailButton;

	public void SetUp()
	{
		GameObject gameObject = base.gameObject;
		gameObject.SetActive(value: true);
		PlayerData player = DataManager.Player;
		PlayerAccountData account = player.account;
		TextMeshPro text;
		TranslationController translationController;
		StringNames id;
		object[] parts;
		if (account.loginStatus != EOSManager.AccountLoginStatus.TempAccount)
		{
			AccountManager instance = DestroyableSingleton<AccountManager>.Instance;
			if (instance.friendsListAllowed == KWSPermissionStatus.Granted)
			{
				return;
			}
			PlayerData player2 = DataManager.Player;
			PlayerAccountData account2 = player2.account;
			if (account2.loginStatus != EOSManager.AccountLoginStatus.LoggedIn)
			{
				PlayerData player3 = DataManager.Player;
				PlayerAccountData account3 = player3.account;
				if (account3.loginStatus != EOSManager.AccountLoginStatus.WaitingForParent)
				{
					return;
				}
			}
			EmailButton.SetActive(value: true);
			text = Text;
			TranslationController instance2 = DestroyableSingleton<TranslationController>.Instance;
			object[] array = Array.Empty<object>();
			translationController = instance2;
			id = StringNames.FriendsListPermissionsWarning;
			parts = array;
		}
		else
		{
			EmailButton.SetActive(value: false);
			text = Text;
			TranslationController instance3 = DestroyableSingleton<TranslationController>.Instance;
			object[] array2 = Array.Empty<object>();
			translationController = instance3;
			id = StringNames.FriendListUnavailable;
			parts = array2;
		}
		string text2 = translationController.GetString(id, parts);
		text.text = text2;
	}
}
