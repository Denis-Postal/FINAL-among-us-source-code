using System;

namespace Epic.OnlineServices.CustomInvites
{
	public sealed class CustomInvitesInterface : Handle
	{
		public const int AcceptrequesttojoinApiLatest = 1;

		public const int AddnotifycustominviteacceptedApiLatest = 1;

		public const int AddnotifycustominvitereceivedApiLatest = 1;

		public const int AddnotifycustominviterejectedApiLatest = 1;

		public const int AddnotifyrequesttojoinacceptedApiLatest = 1;

		public const int AddnotifyrequesttojoinreceivedApiLatest = 1;

		public const int AddnotifyrequesttojoinrejectedApiLatest = 1;

		public const int AddnotifyrequesttojoinresponsereceivedApiLatest = 1;

		public const int AddnotifysendcustomnativeinviterequestedApiLatest = 1;

		public const int FinalizeinviteApiLatest = 1;

		public const int MaxPayloadLength = 500;

		public const int RejectrequesttojoinApiLatest = 1;

		public const int SendcustominviteApiLatest = 1;

		public const int SendrequesttojoinApiLatest = 1;

		public const int SetcustominviteApiLatest = 1;

		public CustomInvitesInterface()
		{
		}

		public CustomInvitesInterface(IntPtr innerHandle)
		{
		}

		public void AcceptRequestToJoin(ref AcceptRequestToJoinOptions options, object clientData, OnAcceptRequestToJoinCallback completionDelegate)
		{
		}

		public ulong AddNotifyCustomInviteAccepted(ref AddNotifyCustomInviteAcceptedOptions options, object clientData, OnCustomInviteAcceptedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyCustomInviteReceived(ref AddNotifyCustomInviteReceivedOptions options, object clientData, OnCustomInviteReceivedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyCustomInviteRejected(ref AddNotifyCustomInviteRejectedOptions options, object clientData, OnCustomInviteRejectedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyRequestToJoinAccepted(ref AddNotifyRequestToJoinAcceptedOptions options, object clientData, OnRequestToJoinAcceptedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyRequestToJoinReceived(ref AddNotifyRequestToJoinReceivedOptions options, object clientData, OnRequestToJoinReceivedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyRequestToJoinRejected(ref AddNotifyRequestToJoinRejectedOptions options, object clientData, OnRequestToJoinRejectedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyRequestToJoinResponseReceived(ref AddNotifyRequestToJoinResponseReceivedOptions options, object clientData, OnRequestToJoinResponseReceivedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifySendCustomNativeInviteRequested(ref AddNotifySendCustomNativeInviteRequestedOptions options, object clientData, OnSendCustomNativeInviteRequestedCallback notificationFn)
		{
			return 0uL;
		}

		public Result FinalizeInvite(ref FinalizeInviteOptions options)
		{
			return Result.Success;
		}

		public void RejectRequestToJoin(ref RejectRequestToJoinOptions options, object clientData, OnRejectRequestToJoinCallback completionDelegate)
		{
		}

		public void RemoveNotifyCustomInviteAccepted(ulong inId)
		{
		}

		public void RemoveNotifyCustomInviteReceived(ulong inId)
		{
		}

		public void RemoveNotifyCustomInviteRejected(ulong inId)
		{
		}

		public void RemoveNotifyRequestToJoinAccepted(ulong inId)
		{
		}

		public void RemoveNotifyRequestToJoinReceived(ulong inId)
		{
		}

		public void RemoveNotifyRequestToJoinRejected(ulong inId)
		{
		}

		public void RemoveNotifyRequestToJoinResponseReceived(ulong inId)
		{
		}

		public void RemoveNotifySendCustomNativeInviteRequested(ulong inId)
		{
		}

		public void SendCustomInvite(ref SendCustomInviteOptions options, object clientData, OnSendCustomInviteCallback completionDelegate)
		{
		}

		public void SendRequestToJoin(ref SendRequestToJoinOptions options, object clientData, OnSendRequestToJoinCallback completionDelegate)
		{
		}

		public Result SetCustomInvite(ref SetCustomInviteOptions options)
		{
			return Result.Success;
		}

		[MonoPInvokeCallback(typeof(OnAcceptRequestToJoinCallbackInternal))]
		internal static void OnAcceptRequestToJoinCallbackInternalImplementation(ref AcceptRequestToJoinCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnCustomInviteAcceptedCallbackInternal))]
		internal static void OnCustomInviteAcceptedCallbackInternalImplementation(ref OnCustomInviteAcceptedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnCustomInviteReceivedCallbackInternal))]
		internal static void OnCustomInviteReceivedCallbackInternalImplementation(ref OnCustomInviteReceivedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnCustomInviteRejectedCallbackInternal))]
		internal static void OnCustomInviteRejectedCallbackInternalImplementation(ref CustomInviteRejectedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnRejectRequestToJoinCallbackInternal))]
		internal static void OnRejectRequestToJoinCallbackInternalImplementation(ref RejectRequestToJoinCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnRequestToJoinAcceptedCallbackInternal))]
		internal static void OnRequestToJoinAcceptedCallbackInternalImplementation(ref OnRequestToJoinAcceptedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnRequestToJoinReceivedCallbackInternal))]
		internal static void OnRequestToJoinReceivedCallbackInternalImplementation(ref RequestToJoinReceivedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnRequestToJoinRejectedCallbackInternal))]
		internal static void OnRequestToJoinRejectedCallbackInternalImplementation(ref OnRequestToJoinRejectedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnRequestToJoinResponseReceivedCallbackInternal))]
		internal static void OnRequestToJoinResponseReceivedCallbackInternalImplementation(ref RequestToJoinResponseReceivedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnSendCustomInviteCallbackInternal))]
		internal static void OnSendCustomInviteCallbackInternalImplementation(ref SendCustomInviteCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnSendCustomNativeInviteRequestedCallbackInternal))]
		internal static void OnSendCustomNativeInviteRequestedCallbackInternalImplementation(ref SendCustomNativeInviteRequestedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnSendRequestToJoinCallbackInternal))]
		internal static void OnSendRequestToJoinCallbackInternalImplementation(ref SendRequestToJoinCallbackInfoInternal data)
		{
		}
	}
}
