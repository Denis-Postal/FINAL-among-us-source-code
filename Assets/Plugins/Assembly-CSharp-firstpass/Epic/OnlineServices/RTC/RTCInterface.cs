using System;
using Epic.OnlineServices.RTCAudio;
using Epic.OnlineServices.RTCData;

namespace Epic.OnlineServices.RTC
{
	public sealed class RTCInterface : Handle
	{
		public const int AddnotifydisconnectedApiLatest = 1;

		public const int AddnotifyparticipantstatuschangedApiLatest = 1;

		public const int AddnotifyroomstatisticsupdatedApiLatest = 1;

		public const int BlockparticipantApiLatest = 1;

		public const int JoinroomApiLatest = 1;

		public const int LeaveroomApiLatest = 1;

		public const int OptionApiLatest = 1;

		public const int OptionKeyMaxcharcount = 256;

		public const int OptionValueMaxcharcount = 256;

		public const int ParticipantmetadataApiLatest = 1;

		public const int ParticipantmetadataKeyMaxcharcount = 256;

		public const int ParticipantmetadataValueMaxcharcount = 256;

		public const int SetroomsettingApiLatest = 1;

		public const int SetsettingApiLatest = 1;

		public RTCInterface()
		{
		}

		public RTCInterface(IntPtr innerHandle)
		{
		}

		public ulong AddNotifyDisconnected(ref AddNotifyDisconnectedOptions options, object clientData, OnDisconnectedCallback completionDelegate)
		{
			return 0uL;
		}

		public ulong AddNotifyParticipantStatusChanged(ref AddNotifyParticipantStatusChangedOptions options, object clientData, OnParticipantStatusChangedCallback completionDelegate)
		{
			return 0uL;
		}

		public ulong AddNotifyRoomStatisticsUpdated(ref AddNotifyRoomStatisticsUpdatedOptions options, object clientData, OnRoomStatisticsUpdatedCallback statisticsUpdateHandler)
		{
			return 0uL;
		}

		public void BlockParticipant(ref BlockParticipantOptions options, object clientData, OnBlockParticipantCallback completionDelegate)
		{
		}

		public RTCAudioInterface GetAudioInterface()
		{
			return null;
		}

		public RTCDataInterface GetDataInterface()
		{
			return null;
		}

		public void JoinRoom(ref JoinRoomOptions options, object clientData, OnJoinRoomCallback completionDelegate)
		{
		}

		public void LeaveRoom(ref LeaveRoomOptions options, object clientData, OnLeaveRoomCallback completionDelegate)
		{
		}

		public void RemoveNotifyDisconnected(ulong notificationId)
		{
		}

		public void RemoveNotifyParticipantStatusChanged(ulong notificationId)
		{
		}

		public void RemoveNotifyRoomStatisticsUpdated(ulong notificationId)
		{
		}

		public Result SetRoomSetting(ref SetRoomSettingOptions options)
		{
			return Result.Success;
		}

		public Result SetSetting(ref SetSettingOptions options)
		{
			return Result.Success;
		}

		[MonoPInvokeCallback(typeof(OnBlockParticipantCallbackInternal))]
		internal static void OnBlockParticipantCallbackInternalImplementation(ref BlockParticipantCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnDisconnectedCallbackInternal))]
		internal static void OnDisconnectedCallbackInternalImplementation(ref DisconnectedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnJoinRoomCallbackInternal))]
		internal static void OnJoinRoomCallbackInternalImplementation(ref JoinRoomCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnLeaveRoomCallbackInternal))]
		internal static void OnLeaveRoomCallbackInternalImplementation(ref LeaveRoomCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnParticipantStatusChangedCallbackInternal))]
		internal static void OnParticipantStatusChangedCallbackInternalImplementation(ref ParticipantStatusChangedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnRoomStatisticsUpdatedCallbackInternal))]
		internal static void OnRoomStatisticsUpdatedCallbackInternalImplementation(ref RoomStatisticsUpdatedInfoInternal data)
		{
		}
	}
}
