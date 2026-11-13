using System;

namespace Epic.OnlineServices.RTCData
{
	public sealed class RTCDataInterface : Handle
	{
		public const int AddnotifydatareceivedApiLatest = 1;

		public const int AddnotifyparticipantupdatedApiLatest = 1;

		public const int MaxPacketSize = 1170;

		public const int SenddataApiLatest = 1;

		public const int UpdatereceivingApiLatest = 1;

		public const int UpdatesendingApiLatest = 1;

		public RTCDataInterface()
		{
		}

		public RTCDataInterface(IntPtr innerHandle)
		{
		}

		public ulong AddNotifyDataReceived(ref AddNotifyDataReceivedOptions options, object clientData, OnDataReceivedCallback completionDelegate)
		{
			return 0uL;
		}

		public ulong AddNotifyParticipantUpdated(ref AddNotifyParticipantUpdatedOptions options, object clientData, OnParticipantUpdatedCallback completionDelegate)
		{
			return 0uL;
		}

		public void RemoveNotifyDataReceived(ulong notificationId)
		{
		}

		public void RemoveNotifyParticipantUpdated(ulong notificationId)
		{
		}

		public Result SendData(ref SendDataOptions options)
		{
			return Result.Success;
		}

		public void UpdateReceiving(ref UpdateReceivingOptions options, object clientData, OnUpdateReceivingCallback completionDelegate)
		{
		}

		public void UpdateSending(ref UpdateSendingOptions options, object clientData, OnUpdateSendingCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnDataReceivedCallbackInternal))]
		internal static void OnDataReceivedCallbackInternalImplementation(ref DataReceivedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnParticipantUpdatedCallbackInternal))]
		internal static void OnParticipantUpdatedCallbackInternalImplementation(ref ParticipantUpdatedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUpdateReceivingCallbackInternal))]
		internal static void OnUpdateReceivingCallbackInternalImplementation(ref UpdateReceivingCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUpdateSendingCallbackInternal))]
		internal static void OnUpdateSendingCallbackInternalImplementation(ref UpdateSendingCallbackInfoInternal data)
		{
		}
	}
}
