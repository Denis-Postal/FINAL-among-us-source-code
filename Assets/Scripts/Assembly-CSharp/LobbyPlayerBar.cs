using System.Collections.Generic;
using Assets.InnerNet;
using InnerNet;
using TMPro;
using UnityEngine;

public class LobbyPlayerBar : FriendsListBar
{
	public TextMeshPro InGameName;

	public SpriteRenderer AddFriendButton;

	public SpriteRenderer BlockButton;

	public SpriteRenderer ReportButton;

	public Sprite BlockedSprite;

	public Sprite FriendSprite;

	public SpriteRenderer StatusSpriteRen;

	private bool IsFriend;

	private bool IsBlocked;

	private bool isReported;

	public UiElement ViewProfileButton;

	public List<PassiveButton> ButtonsToDisableForSelf;

	private FriendsListManager.RecentPlayedWithPlayer recentPlayer;

	private string playerName;

	public override void SetUp(string puid, FriendsListUI parentUI, string friendCode, string playerInGameName)
	{
		base.SetUp(puid, parentUI, friendCode, playerInGameName);
		playerName = playerInGameName;
		InGameName.text = playerInGameName;
		InGameName.fontMaterial.SetFloat("_StencilComp", 3f);
		InGameName.fontMaterial.SetFloat("_Stencil", 51f);
		if (!DestroyableSingleton<EOSManager>.Instance.IsFriendsListAllowed() || (puid == null && friendCode == null) || friendCode == string.Empty)
		{
			AddFriendButton.gameObject.SetActive(value: false);
			BlockButton.gameObject.SetActive(value: false);
		}
		if (DestroyableSingleton<FriendsListManager>.Instance.IsPlayerBlocked(puid))
		{
			IsBlocked = true;
		}
		else if (DestroyableSingleton<FriendsListManager>.Instance.IsPlayerFriend(puid))
		{
			IsFriend = true;
		}
		CheckAlreadyReported();
		UpdateStatus();
	}

	public void SetRecentPlayer(FriendsListManager.RecentPlayedWithPlayer recentP)
	{
		recentPlayer = recentP;
	}

	public void CheckAddFriend()
	{
		if (CanUseApi() && !IsFriend && !IsBlocked)
		{
			actionCooldown = 1f;
			DestroyableSingleton<FriendsListManager>.Instance.OpenConfirmationScreen(AddFriend, FriendsListConfirmMenu.ActionType.Friend, friendCode);
		}
	}

	public void AddFriend()
	{
		GoInactive();
		if (DestroyableSingleton<FriendsListManager>.Instance.ReceivedRequests.FindIndex((ResponseReceivedFriendRequest r) => r.SenderFriendCode == friendCode || r.SenderId == puid) > -1)
		{
			DestroyableSingleton<FriendsListManager>.Instance.Friends.Add(new ResponseFriends
			{
				FriendPuid = puid,
				FriendCode = friendCode
			});
			IsFriend = true;
			UpdateStatus();
		}
		DestroyableSingleton<FriendsListManager>.Instance.SendFriendRequestByUsername(friendCode, (ResponseState cb, Response<ResponseFriendsListRequest> response) =>
		{
			if (cb == ResponseState.Success)
			{
				GoActive();
				UpdateStatus();
				DestroyableSingleton<FriendsListManager>.Instance.FriendListNotification.SetUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FriendRequestSent), FriendListNotification.IconType.Sent);
				parentUI.RefreshFriends();
				parentUI.RefreshNotifications();
			}
			else if (response != null)
			{
				GoActive();
				UpdateStatus();
				if (response.Errors != null && response.Errors.Length != 0 && DestroyableSingleton<FriendsListManager>.Instance.ErrorCodes.ContainsKey(response.Errors[0].Detail))
				{
					DestroyableSingleton<FriendsListManager>.Instance.FriendListNotification.SetUp(DestroyableSingleton<TranslationController>.Instance.GetString(DestroyableSingleton<FriendsListManager>.Instance.ErrorCodes[response.Errors[0].Detail]), FriendListNotification.IconType.Failed);
				}
				else
				{
					DestroyableSingleton<FriendsListManager>.Instance.FriendListNotification.SetUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FriendRequestSentFailed), FriendListNotification.IconType.Failed);
				}
			}
		});
	}

	public void CheckBlockPlayer()
	{
		if (CanUseApi() && !IsBlocked)
		{
			actionCooldown = 1f;
			DestroyableSingleton<FriendsListManager>.Instance.OpenConfirmationScreen(BlockPlayer, FriendsListConfirmMenu.ActionType.Block, friendCode);
		}
	}

	public void BlockPlayer()
	{
		GoInactive();
		IsBlocked = true;
		DestroyableSingleton<FriendsListManager>.Instance.BlockPlayer(puid, (ResponseState cb, Response<ResponseFriendsListRequest> response) =>
		{
			if (cb == ResponseState.Success)
			{
				if (IsFriend)
				{
					IsFriend = false;
					DestroyableSingleton<FriendsListManager>.Instance.RemoveFriend(puid, (ResponseState responseState, Response<ResponseFriendsListRequest> response2) =>
					{
						GoActive();
						UpdateStatus();
						parentUI.UpdateFriendBars();
					});
				}
				else
				{
					GoActive();
					UpdateStatus();
					parentUI.RefreshBlockedPlayers();
				}
				DestroyableSingleton<FriendsListManager>.Instance.BlockedPlayers.Add(new ResponseBlockedPlayer
				{
					FriendPuid = puid,
					FriendCode = friendCode
				});
				DestroyableSingleton<FriendsListManager>.Instance.FriendListNotification.SetUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.BlockedPlayer), FriendListNotification.IconType.Block);
			}
			else if (response != null)
			{
				GoActive();
				if (response.Errors != null && response.Errors.Length != 0 && DestroyableSingleton<FriendsListManager>.Instance.ErrorCodes.ContainsKey(response.Errors[0].Detail))
				{
					DestroyableSingleton<FriendsListManager>.Instance.FriendListNotification.SetUp(DestroyableSingleton<TranslationController>.Instance.GetString(DestroyableSingleton<FriendsListManager>.Instance.ErrorCodes[response.Errors[0].Detail]), FriendListNotification.IconType.Failed);
				}
				else
				{
					DestroyableSingleton<FriendsListManager>.Instance.FriendListNotification.SetUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.BlockedPlayerFailed), FriendListNotification.IconType.Failed);
				}
			}
		});
	}

	public void ReportPlayer()
	{
		if (CanUseApi() && !isReported)
		{
			actionCooldown = 1f;
			DestroyableSingleton<FriendsListManager>.Instance.OpenReportScreen(playerName, 0, recentPlayer);
		}
	}

	public void CheckAlreadyReported()
	{
		if (recentPlayer != null && DestroyableSingleton<FriendsListManager>.Instance.CheckAlreadyReported(recentPlayer.clientId))
		{
			isReported = true;
			UpdateStatus();
		}
	}

	private void DisableButton(SpriteRenderer button)
	{
		button.color = Color.gray;
	}

	private void UpdateStatus()
	{
		if (IsBlocked)
		{
			SenderName.color = Color.red;
			StatusSpriteRen.sprite = BlockedSprite;
			DisableButton(AddFriendButton);
			DisableButton(BlockButton);
		}
		else if (IsFriend)
		{
			StatusSpriteRen.sprite = FriendSprite;
			DisableButton(AddFriendButton);
		}
		if (isReported)
		{
			DisableButton(ReportButton);
		}
	}

	public void SetInfoFromClient(ClientData clientData)
	{
		if ((bool)PlatformIdentifier)
		{
			PlatformIdentifier.SetInfo(clientData);
		}
		if (clientData != null && clientData.PlatformData != null)
		{
			bool flag = false;
			PlatformFriendData = null;
			flag = false;
			if (flag && PlatformIdentifier.iconRenderer.transform.childCount > 0)
			{
				PlatformIdentifier.iconRenderer.transform.GetChild(0).gameObject.SetActive(value: true);
			}
			ViewProfileButton.enabled = flag;
		}
		else
		{
			ViewProfileButton.enabled = false;
		}
	}

	public void DisableButtonsForLocalPlayer()
	{
		foreach (PassiveButton item in ButtonsToDisableForSelf)
		{
			item.gameObject.SetActive(value: false);
		}
	}
}
