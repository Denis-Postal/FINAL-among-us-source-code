using System;

namespace Epic.OnlineServices.Sessions
{
	public sealed class SessionsInterface : Handle
	{
		public const int AddnotifyjoinsessionacceptedApiLatest = 1;

		public const int AddnotifyleavesessionrequestedApiLatest = 1;

		public const int AddnotifysendsessionnativeinviterequestedApiLatest = 1;

		public const int AddnotifysessioninviteacceptedApiLatest = 1;

		public const int AddnotifysessioninvitereceivedApiLatest = 1;

		public const int AddnotifysessioninviterejectedApiLatest = 1;

		public const int AttributedataApiLatest = 1;

		public const int CopyactivesessionhandleApiLatest = 1;

		public const int CopysessionhandlebyinviteidApiLatest = 1;

		public const int CopysessionhandlebyuieventidApiLatest = 1;

		public const int CopysessionhandleforpresenceApiLatest = 1;

		public const int CreatesessionmodificationApiLatest = 5;

		public const int CreatesessionsearchApiLatest = 1;

		public const int DestroysessionApiLatest = 1;

		public const int DumpsessionstateApiLatest = 1;

		public const int EndsessionApiLatest = 1;

		public const int GetinvitecountApiLatest = 1;

		public const int GetinviteidbyindexApiLatest = 1;

		public const int InviteidMaxLength = 64;

		public const int IsuserinsessionApiLatest = 1;

		public const int JoinsessionApiLatest = 2;

		public const int MaxSearchResults = 200;

		public const int Maxregisteredplayers = 1000;

		public const int QueryinvitesApiLatest = 1;

		public const int RegisterplayersApiLatest = 3;

		public const int RejectinviteApiLatest = 1;

		public static readonly Utf8String SearchBucketId;

		public static readonly Utf8String SearchEmptyServersOnly;

		public static readonly Utf8String SearchMinslotsavailable;

		public static readonly Utf8String SearchNonemptyServersOnly;

		public const int SendinviteApiLatest = 1;

		public const int SessionattributeApiLatest = 1;

		public const int SessionattributedataApiLatest = 1;

		public const int StartsessionApiLatest = 1;

		public const int UnregisterplayersApiLatest = 2;

		public const int UpdatesessionApiLatest = 1;

		public const int UpdatesessionmodificationApiLatest = 1;

		public SessionsInterface()
		{
		}

		public SessionsInterface(IntPtr innerHandle)
		{
		}

		public ulong AddNotifyJoinSessionAccepted(ref AddNotifyJoinSessionAcceptedOptions options, object clientData, OnJoinSessionAcceptedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyLeaveSessionRequested(ref AddNotifyLeaveSessionRequestedOptions options, object clientData, OnLeaveSessionRequestedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifySendSessionNativeInviteRequested(ref AddNotifySendSessionNativeInviteRequestedOptions options, object clientData, OnSendSessionNativeInviteRequestedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifySessionInviteAccepted(ref AddNotifySessionInviteAcceptedOptions options, object clientData, OnSessionInviteAcceptedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifySessionInviteReceived(ref AddNotifySessionInviteReceivedOptions options, object clientData, OnSessionInviteReceivedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifySessionInviteRejected(ref AddNotifySessionInviteRejectedOptions options, object clientData, OnSessionInviteRejectedCallback notificationFn)
		{
			return 0uL;
		}

		public Result CopyActiveSessionHandle(ref CopyActiveSessionHandleOptions options, out ActiveSession outSessionHandle)
		{
			outSessionHandle = null;
			return Result.Success;
		}

		public Result CopySessionHandleByInviteId(ref CopySessionHandleByInviteIdOptions options, out SessionDetails outSessionHandle)
		{
			outSessionHandle = null;
			return Result.Success;
		}

		public Result CopySessionHandleByUiEventId(ref CopySessionHandleByUiEventIdOptions options, out SessionDetails outSessionHandle)
		{
			outSessionHandle = null;
			return Result.Success;
		}

		public Result CopySessionHandleForPresence(ref CopySessionHandleForPresenceOptions options, out SessionDetails outSessionHandle)
		{
			outSessionHandle = null;
			return Result.Success;
		}

		public Result CreateSessionModification(ref CreateSessionModificationOptions options, out SessionModification outSessionModificationHandle)
		{
			outSessionModificationHandle = null;
			return Result.Success;
		}

		public Result CreateSessionSearch(ref CreateSessionSearchOptions options, out SessionSearch outSessionSearchHandle)
		{
			outSessionSearchHandle = null;
			return Result.Success;
		}

		public void DestroySession(ref DestroySessionOptions options, object clientData, OnDestroySessionCallback completionDelegate)
		{
		}

		public Result DumpSessionState(ref DumpSessionStateOptions options)
		{
			return Result.Success;
		}

		public void EndSession(ref EndSessionOptions options, object clientData, OnEndSessionCallback completionDelegate)
		{
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

		public Result IsUserInSession(ref IsUserInSessionOptions options)
		{
			return Result.Success;
		}

		public void JoinSession(ref JoinSessionOptions options, object clientData, OnJoinSessionCallback completionDelegate)
		{
		}

		public void QueryInvites(ref QueryInvitesOptions options, object clientData, OnQueryInvitesCallback completionDelegate)
		{
		}

		public void RegisterPlayers(ref RegisterPlayersOptions options, object clientData, OnRegisterPlayersCallback completionDelegate)
		{
		}

		public void RejectInvite(ref RejectInviteOptions options, object clientData, OnRejectInviteCallback completionDelegate)
		{
		}

		public void RemoveNotifyJoinSessionAccepted(ulong inId)
		{
		}

		public void RemoveNotifyLeaveSessionRequested(ulong inId)
		{
		}

		public void RemoveNotifySendSessionNativeInviteRequested(ulong inId)
		{
		}

		public void RemoveNotifySessionInviteAccepted(ulong inId)
		{
		}

		public void RemoveNotifySessionInviteReceived(ulong inId)
		{
		}

		public void RemoveNotifySessionInviteRejected(ulong inId)
		{
		}

		public void SendInvite(ref SendInviteOptions options, object clientData, OnSendInviteCallback completionDelegate)
		{
		}

		public void StartSession(ref StartSessionOptions options, object clientData, OnStartSessionCallback completionDelegate)
		{
		}

		public void UnregisterPlayers(ref UnregisterPlayersOptions options, object clientData, OnUnregisterPlayersCallback completionDelegate)
		{
		}

		public void UpdateSession(ref UpdateSessionOptions options, object clientData, OnUpdateSessionCallback completionDelegate)
		{
		}

		public Result UpdateSessionModification(ref UpdateSessionModificationOptions options, out SessionModification outSessionModificationHandle)
		{
			outSessionModificationHandle = null;
			return Result.Success;
		}

		[MonoPInvokeCallback(typeof(OnDestroySessionCallbackInternal))]
		internal static void OnDestroySessionCallbackInternalImplementation(ref DestroySessionCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnEndSessionCallbackInternal))]
		internal static void OnEndSessionCallbackInternalImplementation(ref EndSessionCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnJoinSessionAcceptedCallbackInternal))]
		internal static void OnJoinSessionAcceptedCallbackInternalImplementation(ref JoinSessionAcceptedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnJoinSessionCallbackInternal))]
		internal static void OnJoinSessionCallbackInternalImplementation(ref JoinSessionCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnLeaveSessionRequestedCallbackInternal))]
		internal static void OnLeaveSessionRequestedCallbackInternalImplementation(ref LeaveSessionRequestedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryInvitesCallbackInternal))]
		internal static void OnQueryInvitesCallbackInternalImplementation(ref QueryInvitesCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnRegisterPlayersCallbackInternal))]
		internal static void OnRegisterPlayersCallbackInternalImplementation(ref RegisterPlayersCallbackInfoInternal data)
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

		[MonoPInvokeCallback(typeof(OnSendSessionNativeInviteRequestedCallbackInternal))]
		internal static void OnSendSessionNativeInviteRequestedCallbackInternalImplementation(ref SendSessionNativeInviteRequestedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnSessionInviteAcceptedCallbackInternal))]
		internal static void OnSessionInviteAcceptedCallbackInternalImplementation(ref SessionInviteAcceptedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnSessionInviteReceivedCallbackInternal))]
		internal static void OnSessionInviteReceivedCallbackInternalImplementation(ref SessionInviteReceivedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnSessionInviteRejectedCallbackInternal))]
		internal static void OnSessionInviteRejectedCallbackInternalImplementation(ref SessionInviteRejectedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnStartSessionCallbackInternal))]
		internal static void OnStartSessionCallbackInternalImplementation(ref StartSessionCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUnregisterPlayersCallbackInternal))]
		internal static void OnUnregisterPlayersCallbackInternalImplementation(ref UnregisterPlayersCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUpdateSessionCallbackInternal))]
		internal static void OnUpdateSessionCallbackInternalImplementation(ref UpdateSessionCallbackInfoInternal data)
		{
		}
	}
}
