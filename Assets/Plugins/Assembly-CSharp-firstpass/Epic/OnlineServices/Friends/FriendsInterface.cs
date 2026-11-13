using System;

namespace Epic.OnlineServices.Friends
{
	public sealed class FriendsInterface : Handle
	{
		public const int AcceptinviteApiLatest = 1;

		public const int AddnotifyblockedusersupdateApiLatest = 1;

		public const int AddnotifyfriendsupdateApiLatest = 1;

		public const int GetblockeduseratindexApiLatest = 1;

		public const int GetblockeduserscountApiLatest = 1;

		public const int GetfriendatindexApiLatest = 1;

		public const int GetfriendscountApiLatest = 1;

		public const int GetstatusApiLatest = 1;

		public const int QueryfriendsApiLatest = 1;

		public const int RejectinviteApiLatest = 1;

		public const int SendinviteApiLatest = 1;

		public FriendsInterface()
		{
		}

		public FriendsInterface(IntPtr innerHandle)
		{
		}

		public void AcceptInvite(ref AcceptInviteOptions options, object clientData, OnAcceptInviteCallback completionDelegate)
		{
		}

		public ulong AddNotifyBlockedUsersUpdate(ref AddNotifyBlockedUsersUpdateOptions options, object clientData, OnBlockedUsersUpdateCallback blockedUsersUpdateHandler)
		{
			return 0uL;
		}

		public ulong AddNotifyFriendsUpdate(ref AddNotifyFriendsUpdateOptions options, object clientData, OnFriendsUpdateCallback friendsUpdateHandler)
		{
			return 0uL;
		}

		public EpicAccountId GetBlockedUserAtIndex(ref GetBlockedUserAtIndexOptions options)
		{
			return null;
		}

		public int GetBlockedUsersCount(ref GetBlockedUsersCountOptions options)
		{
			return 0;
		}

		public EpicAccountId GetFriendAtIndex(ref GetFriendAtIndexOptions options)
		{
			return null;
		}

		public int GetFriendsCount(ref GetFriendsCountOptions options)
		{
			return 0;
		}

		public FriendsStatus GetStatus(ref GetStatusOptions options)
		{
			return FriendsStatus.NotFriends;
		}

		public void QueryFriends(ref QueryFriendsOptions options, object clientData, OnQueryFriendsCallback completionDelegate)
		{
		}

		public void RejectInvite(ref RejectInviteOptions options, object clientData, OnRejectInviteCallback completionDelegate)
		{
		}

		public void RemoveNotifyBlockedUsersUpdate(ulong notificationId)
		{
		}

		public void RemoveNotifyFriendsUpdate(ulong notificationId)
		{
		}

		public void SendInvite(ref SendInviteOptions options, object clientData, OnSendInviteCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnAcceptInviteCallbackInternal))]
		internal static void OnAcceptInviteCallbackInternalImplementation(ref AcceptInviteCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnBlockedUsersUpdateCallbackInternal))]
		internal static void OnBlockedUsersUpdateCallbackInternalImplementation(ref OnBlockedUsersUpdateInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnFriendsUpdateCallbackInternal))]
		internal static void OnFriendsUpdateCallbackInternalImplementation(ref OnFriendsUpdateInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryFriendsCallbackInternal))]
		internal static void OnQueryFriendsCallbackInternalImplementation(ref QueryFriendsCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnRejectInviteCallbackInternal))]
		internal static void OnRejectInviteCallbackInternalImplementation(ref RejectInviteCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnSendInviteCallbackInternal))]
		internal static void OnSendInviteCallbackInternalImplementation(ref SendInviteCallbackInfoInternal data)
		{
		}
	}
}
