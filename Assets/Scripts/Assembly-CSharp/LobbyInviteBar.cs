using Object = UnityEngine.Object;
using System;
using Assets.InnerNet;
using UnityEngine;

public class LobbyInviteBar : FriendsListBar
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Action<ResponseState, Response<ResponseFriendsListRequest>> _003C_003E9__4_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal void _003CDenyLobbyInvite_003Eb__4_0(ResponseState cb, Response<ResponseFriendsListRequest> response)
		{
			if (cb != ResponseState.Success)
			{
				Debug.Log("Deny lobby invite FAILED");
			}
			else
			{
				Debug.Log("Deny lobby invite successful");
			}
		}
	}

	private string roomCode;

	private bool hasAccepted;

	public void SetUp(string puid, FriendsListUI parentUI, string roomCode, string friendCode, string playerName)
	{
		SetUp(puid, parentUI, friendCode, playerName);
		this.roomCode = roomCode;
		hasAccepted = false;
	}

	public void PressAccept()
	{
		if (!hasAccepted)
		{
			hasAccepted = true;
			DestroyableSingleton<FriendsListManager>.Instance.JoinGameViaFriendInvite(roomCode);
			parentUI.Close(force: true);
			DenyLobbyInvite();
		}
	}

	public void DenyLobbyInvite()
	{
		DestroyableSingleton<FriendsListManager>.Instance.DenyLobbyInvite(puid, (ResponseState cb, Response<ResponseFriendsListRequest> response) =>
		{
			if (cb == ResponseState.Success)
			{
				Debug.Log("Deny lobby invite successful");
			}
			else
			{
				Debug.Log("Deny lobby invite FAILED");
			}
		});
		parentUI.RefreshFriends();
		Object.Destroy(gameObject);
	}
}
