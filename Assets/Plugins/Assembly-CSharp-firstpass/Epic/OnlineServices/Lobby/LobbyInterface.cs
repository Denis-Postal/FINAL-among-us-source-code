using System;

namespace Epic.OnlineServices.Lobby
{
	public sealed class LobbyInterface : Handle
	{
		public const int AddnotifyjoinlobbyacceptedApiLatest = 1;

		public const int AddnotifyleavelobbyrequestedApiLatest = 1;

		public const int AddnotifylobbyinviteacceptedApiLatest = 1;

		public const int AddnotifylobbyinvitereceivedApiLatest = 1;

		public const int AddnotifylobbyinviterejectedApiLatest = 1;

		public const int AddnotifylobbymemberstatusreceivedApiLatest = 1;

		public const int AddnotifylobbymemberupdatereceivedApiLatest = 1;

		public const int AddnotifylobbyupdatereceivedApiLatest = 1;

		public const int AddnotifyrtcroomconnectionchangedApiLatest = 2;

		public const int AddnotifysendlobbynativeinviterequestedApiLatest = 1;

		public const int AttributeApiLatest = 1;

		public const int AttributedataApiLatest = 1;

		public const int CopylobbydetailshandleApiLatest = 1;

		public const int CopylobbydetailshandlebyinviteidApiLatest = 1;

		public const int CopylobbydetailshandlebyuieventidApiLatest = 1;

		public const int CreatelobbyApiLatest = 9;

		public const int CreatelobbysearchApiLatest = 1;

		public const int DestroylobbyApiLatest = 1;

		public const int GetconnectstringApiLatest = 1;

		public const int GetconnectstringBufferSize = 256;

		public const int GetinvitecountApiLatest = 1;

		public const int GetinviteidbyindexApiLatest = 1;

		public const int GetrtcroomnameApiLatest = 1;

		public const int HardmutememberApiLatest = 1;

		public const int InviteidMaxLength = 64;

		public const int IsrtcroomconnectedApiLatest = 1;

		public const int JoinlobbyApiLatest = 4;

		public const int JoinlobbybyidApiLatest = 2;

		public const int KickmemberApiLatest = 1;

		public const int LeavelobbyApiLatest = 1;

		public const int LocalrtcoptionsApiLatest = 1;

		public const int MaxLobbies = 16;

		public const int MaxLobbyMembers = 64;

		public const int MaxLobbyidoverrideLength = 60;

		public const int MaxSearchResults = 200;

		public const int MinLobbyidoverrideLength = 4;

		public const int ParseconnectstringApiLatest = 1;

		public const int ParseconnectstringBufferSize = 256;

		public const int PromotememberApiLatest = 1;

		public const int QueryinvitesApiLatest = 1;

		public const int RejectinviteApiLatest = 1;

		public static readonly Utf8String SearchBucketId;

		public static readonly Utf8String SearchMincurrentmembers;

		public static readonly Utf8String SearchMinslotsavailable;

		public const int SendinviteApiLatest = 1;

		public const int UpdatelobbyApiLatest = 1;

		public const int UpdatelobbymodificationApiLatest = 1;

		public LobbyInterface()
		{
		}

		public LobbyInterface(IntPtr innerHandle)
		{
		}

		public ulong AddNotifyJoinLobbyAccepted(ref AddNotifyJoinLobbyAcceptedOptions options, object clientData, OnJoinLobbyAcceptedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyLeaveLobbyRequested(ref AddNotifyLeaveLobbyRequestedOptions options, object clientData, OnLeaveLobbyRequestedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyLobbyInviteAccepted(ref AddNotifyLobbyInviteAcceptedOptions options, object clientData, OnLobbyInviteAcceptedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyLobbyInviteReceived(ref AddNotifyLobbyInviteReceivedOptions options, object clientData, OnLobbyInviteReceivedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyLobbyInviteRejected(ref AddNotifyLobbyInviteRejectedOptions options, object clientData, OnLobbyInviteRejectedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyLobbyMemberStatusReceived(ref AddNotifyLobbyMemberStatusReceivedOptions options, object clientData, OnLobbyMemberStatusReceivedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyLobbyMemberUpdateReceived(ref AddNotifyLobbyMemberUpdateReceivedOptions options, object clientData, OnLobbyMemberUpdateReceivedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyLobbyUpdateReceived(ref AddNotifyLobbyUpdateReceivedOptions options, object clientData, OnLobbyUpdateReceivedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyRTCRoomConnectionChanged(ref AddNotifyRTCRoomConnectionChangedOptions options, object clientData, OnRTCRoomConnectionChangedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifySendLobbyNativeInviteRequested(ref AddNotifySendLobbyNativeInviteRequestedOptions options, object clientData, OnSendLobbyNativeInviteRequestedCallback notificationFn)
		{
			return 0uL;
		}

		public Result CopyLobbyDetailsHandle(ref CopyLobbyDetailsHandleOptions options, out LobbyDetails outLobbyDetailsHandle)
		{
			outLobbyDetailsHandle = null;
			return Result.Success;
		}

		public Result CopyLobbyDetailsHandleByInviteId(ref CopyLobbyDetailsHandleByInviteIdOptions options, out LobbyDetails outLobbyDetailsHandle)
		{
			outLobbyDetailsHandle = null;
			return Result.Success;
		}

		public Result CopyLobbyDetailsHandleByUiEventId(ref CopyLobbyDetailsHandleByUiEventIdOptions options, out LobbyDetails outLobbyDetailsHandle)
		{
			outLobbyDetailsHandle = null;
			return Result.Success;
		}

		public void CreateLobby(ref CreateLobbyOptions options, object clientData, OnCreateLobbyCallback completionDelegate)
		{
		}

		public Result CreateLobbySearch(ref CreateLobbySearchOptions options, out LobbySearch outLobbySearchHandle)
		{
			outLobbySearchHandle = null;
			return Result.Success;
		}

		public void DestroyLobby(ref DestroyLobbyOptions options, object clientData, OnDestroyLobbyCallback completionDelegate)
		{
		}

		public Result GetConnectString(ref GetConnectStringOptions options, out Utf8String outBuffer)
		{
			outBuffer = null;
			return Result.Success;
		}

		public uint GetInviteCount(ref GetInviteCountOptions options)
		{
			return 0u;
		}

		public Result GetInviteIdByIndex(ref GetInviteIdByIndexOptions options, out Utf8String outBuffer)
		{
			outBuffer = null;
			return Result.Success;
		}

		public Result GetRTCRoomName(ref GetRTCRoomNameOptions options, out Utf8String outBuffer)
		{
			outBuffer = null;
			return Result.Success;
		}

		public void HardMuteMember(ref HardMuteMemberOptions options, object clientData, OnHardMuteMemberCallback completionDelegate)
		{
		}

		public Result IsRTCRoomConnected(ref IsRTCRoomConnectedOptions options, out bool bOutIsConnected)
		{
			bOutIsConnected = default;
			return Result.Success;
		}

		public void JoinLobby(ref JoinLobbyOptions options, object clientData, OnJoinLobbyCallback completionDelegate)
		{
		}

		public void JoinLobbyById(ref JoinLobbyByIdOptions options, object clientData, OnJoinLobbyByIdCallback completionDelegate)
		{
		}

		public void KickMember(ref KickMemberOptions options, object clientData, OnKickMemberCallback completionDelegate)
		{
		}

		public void LeaveLobby(ref LeaveLobbyOptions options, object clientData, OnLeaveLobbyCallback completionDelegate)
		{
		}

		public Result ParseConnectString(ref ParseConnectStringOptions options, out Utf8String outBuffer)
		{
			outBuffer = null;
			return Result.Success;
		}

		public void PromoteMember(ref PromoteMemberOptions options, object clientData, OnPromoteMemberCallback completionDelegate)
		{
		}

		public void QueryInvites(ref QueryInvitesOptions options, object clientData, OnQueryInvitesCallback completionDelegate)
		{
		}

		public void RejectInvite(ref RejectInviteOptions options, object clientData, OnRejectInviteCallback completionDelegate)
		{
		}

		public void RemoveNotifyJoinLobbyAccepted(ulong inId)
		{
		}

		public void RemoveNotifyLeaveLobbyRequested(ulong inId)
		{
		}

		public void RemoveNotifyLobbyInviteAccepted(ulong inId)
		{
		}

		public void RemoveNotifyLobbyInviteReceived(ulong inId)
		{
		}

		public void RemoveNotifyLobbyInviteRejected(ulong inId)
		{
		}

		public void RemoveNotifyLobbyMemberStatusReceived(ulong inId)
		{
		}

		public void RemoveNotifyLobbyMemberUpdateReceived(ulong inId)
		{
		}

		public void RemoveNotifyLobbyUpdateReceived(ulong inId)
		{
		}

		public void RemoveNotifyRTCRoomConnectionChanged(ulong inId)
		{
		}

		public void RemoveNotifySendLobbyNativeInviteRequested(ulong inId)
		{
		}

		public void SendInvite(ref SendInviteOptions options, object clientData, OnSendInviteCallback completionDelegate)
		{
		}

		public void UpdateLobby(ref UpdateLobbyOptions options, object clientData, OnUpdateLobbyCallback completionDelegate)
		{
		}

		public Result UpdateLobbyModification(ref UpdateLobbyModificationOptions options, out LobbyModification outLobbyModificationHandle)
		{
			outLobbyModificationHandle = null;
			return Result.Success;
		}

		[MonoPInvokeCallback(typeof(OnCreateLobbyCallbackInternal))]
		internal static void OnCreateLobbyCallbackInternalImplementation(ref CreateLobbyCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnDestroyLobbyCallbackInternal))]
		internal static void OnDestroyLobbyCallbackInternalImplementation(ref DestroyLobbyCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnHardMuteMemberCallbackInternal))]
		internal static void OnHardMuteMemberCallbackInternalImplementation(ref HardMuteMemberCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnJoinLobbyAcceptedCallbackInternal))]
		internal static void OnJoinLobbyAcceptedCallbackInternalImplementation(ref JoinLobbyAcceptedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnJoinLobbyByIdCallbackInternal))]
		internal static void OnJoinLobbyByIdCallbackInternalImplementation(ref JoinLobbyByIdCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnJoinLobbyCallbackInternal))]
		internal static void OnJoinLobbyCallbackInternalImplementation(ref JoinLobbyCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnKickMemberCallbackInternal))]
		internal static void OnKickMemberCallbackInternalImplementation(ref KickMemberCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnLeaveLobbyCallbackInternal))]
		internal static void OnLeaveLobbyCallbackInternalImplementation(ref LeaveLobbyCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnLeaveLobbyRequestedCallbackInternal))]
		internal static void OnLeaveLobbyRequestedCallbackInternalImplementation(ref LeaveLobbyRequestedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnLobbyInviteAcceptedCallbackInternal))]
		internal static void OnLobbyInviteAcceptedCallbackInternalImplementation(ref LobbyInviteAcceptedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnLobbyInviteReceivedCallbackInternal))]
		internal static void OnLobbyInviteReceivedCallbackInternalImplementation(ref LobbyInviteReceivedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnLobbyInviteRejectedCallbackInternal))]
		internal static void OnLobbyInviteRejectedCallbackInternalImplementation(ref LobbyInviteRejectedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnLobbyMemberStatusReceivedCallbackInternal))]
		internal static void OnLobbyMemberStatusReceivedCallbackInternalImplementation(ref LobbyMemberStatusReceivedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnLobbyMemberUpdateReceivedCallbackInternal))]
		internal static void OnLobbyMemberUpdateReceivedCallbackInternalImplementation(ref LobbyMemberUpdateReceivedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnLobbyUpdateReceivedCallbackInternal))]
		internal static void OnLobbyUpdateReceivedCallbackInternalImplementation(ref LobbyUpdateReceivedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnPromoteMemberCallbackInternal))]
		internal static void OnPromoteMemberCallbackInternalImplementation(ref PromoteMemberCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryInvitesCallbackInternal))]
		internal static void OnQueryInvitesCallbackInternalImplementation(ref QueryInvitesCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnRTCRoomConnectionChangedCallbackInternal))]
		internal static void OnRTCRoomConnectionChangedCallbackInternalImplementation(ref RTCRoomConnectionChangedCallbackInfoInternal data)
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

		[MonoPInvokeCallback(typeof(OnSendLobbyNativeInviteRequestedCallbackInternal))]
		internal static void OnSendLobbyNativeInviteRequestedCallbackInternalImplementation(ref SendLobbyNativeInviteRequestedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUpdateLobbyCallbackInternal))]
		internal static void OnUpdateLobbyCallbackInternalImplementation(ref UpdateLobbyCallbackInfoInternal data)
		{
		}
	}
}
