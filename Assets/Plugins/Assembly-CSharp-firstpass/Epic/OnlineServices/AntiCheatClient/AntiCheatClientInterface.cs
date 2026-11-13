using System;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatClient
{
	public sealed class AntiCheatClientInterface : Handle
	{
		public const int AddexternalintegritycatalogApiLatest = 1;

		public const int AddnotifyclientintegrityviolatedApiLatest = 1;

		public const int AddnotifymessagetopeerApiLatest = 1;

		public const int AddnotifymessagetoserverApiLatest = 1;

		public const int AddnotifypeeractionrequiredApiLatest = 1;

		public const int AddnotifypeerauthstatuschangedApiLatest = 1;

		public const int BeginsessionApiLatest = 3;

		public const int EndsessionApiLatest = 1;

		public const int GetprotectmessageoutputlengthApiLatest = 1;

		public const int OnmessagetopeercallbackMaxMessageSize = 512;

		public const int OnmessagetoservercallbackMaxMessageSize = 512;

		public IntPtr PeerSelf;

		public const int PollstatusApiLatest = 1;

		public const int ProtectmessageApiLatest = 1;

		public const int ReceivemessagefrompeerApiLatest = 1;

		public const int ReceivemessagefromserverApiLatest = 1;

		public const int RegisterpeerApiLatest = 3;

		public const int RegisterpeerMaxAuthenticationtimeout = 120;

		public const int RegisterpeerMinAuthenticationtimeout = 40;

		public const int UnprotectmessageApiLatest = 1;

		public const int UnregisterpeerApiLatest = 1;

		public AntiCheatClientInterface()
		{
		}

		public AntiCheatClientInterface(IntPtr innerHandle)
		{
		}

		public Result AddExternalIntegrityCatalog(ref AddExternalIntegrityCatalogOptions options)
		{
			return Result.Success;
		}

		public ulong AddNotifyClientIntegrityViolated(ref AddNotifyClientIntegrityViolatedOptions options, object clientData, OnClientIntegrityViolatedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyMessageToPeer(ref AddNotifyMessageToPeerOptions options, object clientData, OnMessageToPeerCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyMessageToServer(ref AddNotifyMessageToServerOptions options, object clientData, OnMessageToServerCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyPeerActionRequired(ref AddNotifyPeerActionRequiredOptions options, object clientData, OnPeerActionRequiredCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyPeerAuthStatusChanged(ref AddNotifyPeerAuthStatusChangedOptions options, object clientData, OnPeerAuthStatusChangedCallback notificationFn)
		{
			return 0uL;
		}

		public Result BeginSession(ref BeginSessionOptions options)
		{
			return Result.Success;
		}

		public Result EndSession(ref EndSessionOptions options)
		{
			return Result.Success;
		}

		public Result GetProtectMessageOutputLength(ref GetProtectMessageOutputLengthOptions options, out uint outBufferSizeBytes)
		{
			outBufferSizeBytes = default;
			return Result.Success;
		}

		public Result PollStatus(ref PollStatusOptions options, out AntiCheatClientViolationType outViolationType, out Utf8String outMessage)
		{
			outViolationType = default;
			outMessage = null;
			return Result.Success;
		}

		public Result ProtectMessage(ref ProtectMessageOptions options, ArraySegment<byte> outBuffer, out uint outBytesWritten)
		{
			outBytesWritten = default;
			return Result.Success;
		}

		public Result ReceiveMessageFromPeer(ref ReceiveMessageFromPeerOptions options)
		{
			return Result.Success;
		}

		public Result ReceiveMessageFromServer(ref ReceiveMessageFromServerOptions options)
		{
			return Result.Success;
		}

		public Result RegisterPeer(ref RegisterPeerOptions options)
		{
			return Result.Success;
		}

		public void RemoveNotifyClientIntegrityViolated(ulong notificationId)
		{
		}

		public void RemoveNotifyMessageToPeer(ulong notificationId)
		{
		}

		public void RemoveNotifyMessageToServer(ulong notificationId)
		{
		}

		public void RemoveNotifyPeerActionRequired(ulong notificationId)
		{
		}

		public void RemoveNotifyPeerAuthStatusChanged(ulong notificationId)
		{
		}

		public Result UnprotectMessage(ref UnprotectMessageOptions options, ArraySegment<byte> outBuffer, out uint outBytesWritten)
		{
			outBytesWritten = default;
			return Result.Success;
		}

		public Result UnregisterPeer(ref UnregisterPeerOptions options)
		{
			return Result.Success;
		}

		[MonoPInvokeCallback(typeof(OnClientIntegrityViolatedCallbackInternal))]
		internal static void OnClientIntegrityViolatedCallbackInternalImplementation(ref OnClientIntegrityViolatedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnMessageToPeerCallbackInternal))]
		internal static void OnMessageToPeerCallbackInternalImplementation(ref OnMessageToClientCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnMessageToServerCallbackInternal))]
		internal static void OnMessageToServerCallbackInternalImplementation(ref OnMessageToServerCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnPeerActionRequiredCallbackInternal))]
		internal static void OnPeerActionRequiredCallbackInternalImplementation(ref OnClientActionRequiredCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnPeerAuthStatusChangedCallbackInternal))]
		internal static void OnPeerAuthStatusChangedCallbackInternalImplementation(ref OnClientAuthStatusChangedCallbackInfoInternal data)
		{
		}
	}
}
