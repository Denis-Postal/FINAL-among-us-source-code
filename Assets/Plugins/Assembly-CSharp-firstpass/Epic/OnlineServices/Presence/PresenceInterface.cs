using System;

namespace Epic.OnlineServices.Presence
{
	public sealed class PresenceInterface : Handle
	{
		public const int AddnotifyjoingameacceptedApiLatest = 2;

		public const int AddnotifyonpresencechangedApiLatest = 1;

		public const int CopypresenceApiLatest = 3;

		public const int CreatepresencemodificationApiLatest = 1;

		public const int DataMaxKeyLength = 64;

		public const int DataMaxKeys = 32;

		public const int DataMaxValueLength = 255;

		public const int DatarecordApiLatest = 1;

		public const int DeletedataApiLatest = 1;

		public const int GetjoininfoApiLatest = 1;

		public const int HaspresenceApiLatest = 1;

		public const int InfoApiLatest = 3;

		public static readonly Utf8String KeyPlatformPresence;

		public const int QuerypresenceApiLatest = 1;

		public const int RichTextMaxValueLength = 255;

		public const int SetdataApiLatest = 1;

		public const int SetpresenceApiLatest = 1;

		public const int SetrawrichtextApiLatest = 1;

		public const int SetstatusApiLatest = 1;

		public PresenceInterface()
		{
		}

		public PresenceInterface(IntPtr innerHandle)
		{
		}

		public ulong AddNotifyJoinGameAccepted(ref AddNotifyJoinGameAcceptedOptions options, object clientData, OnJoinGameAcceptedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyOnPresenceChanged(ref AddNotifyOnPresenceChangedOptions options, object clientData, OnPresenceChangedCallback notificationHandler)
		{
			return 0uL;
		}

		public Result CopyPresence(ref CopyPresenceOptions options, out Info? outPresence)
		{
			outPresence = null;
			return Result.Success;
		}

		public Result CreatePresenceModification(ref CreatePresenceModificationOptions options, out PresenceModification outPresenceModificationHandle)
		{
			outPresenceModificationHandle = null;
			return Result.Success;
		}

		public Result GetJoinInfo(ref GetJoinInfoOptions options, out Utf8String outBuffer)
		{
			outBuffer = null;
			return Result.Success;
		}

		public bool HasPresence(ref HasPresenceOptions options)
		{
			return false;
		}

		public void QueryPresence(ref QueryPresenceOptions options, object clientData, OnQueryPresenceCompleteCallback completionDelegate)
		{
		}

		public void RemoveNotifyJoinGameAccepted(ulong inId)
		{
		}

		public void RemoveNotifyOnPresenceChanged(ulong notificationId)
		{
		}

		public void SetPresence(ref SetPresenceOptions options, object clientData, SetPresenceCompleteCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnJoinGameAcceptedCallbackInternal))]
		internal static void OnJoinGameAcceptedCallbackInternalImplementation(ref JoinGameAcceptedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnPresenceChangedCallbackInternal))]
		internal static void OnPresenceChangedCallbackInternalImplementation(ref PresenceChangedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryPresenceCompleteCallbackInternal))]
		internal static void OnQueryPresenceCompleteCallbackInternalImplementation(ref QueryPresenceCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(SetPresenceCompleteCallbackInternal))]
		internal static void SetPresenceCompleteCallbackInternalImplementation(ref SetPresenceCallbackInfoInternal data)
		{
		}
	}
}
