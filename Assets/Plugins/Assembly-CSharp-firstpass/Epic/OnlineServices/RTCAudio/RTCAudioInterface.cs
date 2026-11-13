using System;

namespace Epic.OnlineServices.RTCAudio
{
	public sealed class RTCAudioInterface : Handle
	{
		public const int AddnotifyaudiobeforerenderApiLatest = 1;

		public const int AddnotifyaudiobeforesendApiLatest = 1;

		public const int AddnotifyaudiodeviceschangedApiLatest = 1;

		public const int AddnotifyaudioinputstateApiLatest = 1;

		public const int AddnotifyaudiooutputstateApiLatest = 1;

		public const int AddnotifyparticipantupdatedApiLatest = 1;

		public const int AudiobufferApiLatest = 1;

		public const int AudioinputdeviceinfoApiLatest = 1;

		public const int AudiooutputdeviceinfoApiLatest = 1;

		public const int CopyinputdeviceinformationbyindexApiLatest = 1;

		public const int CopyoutputdeviceinformationbyindexApiLatest = 1;

		public const int GetaudioinputdevicebyindexApiLatest = 1;

		public const int GetaudioinputdevicescountApiLatest = 1;

		public const int GetaudiooutputdevicebyindexApiLatest = 1;

		public const int GetaudiooutputdevicescountApiLatest = 1;

		public const int GetinputdevicescountApiLatest = 1;

		public const int GetoutputdevicescountApiLatest = 1;

		public const int InputdeviceinformationApiLatest = 1;

		public const int OutputdeviceinformationApiLatest = 1;

		public const int QueryinputdevicesinformationApiLatest = 1;

		public const int QueryoutputdevicesinformationApiLatest = 1;

		public const int RegisterplatformaudiouserApiLatest = 1;

		public const int RegisterplatformuserApiLatest = 1;

		public const int SendaudioApiLatest = 1;

		public const int SetaudioinputsettingsApiLatest = 1;

		public const int SetaudiooutputsettingsApiLatest = 1;

		public const int SetinputdevicesettingsApiLatest = 1;

		public const int SetoutputdevicesettingsApiLatest = 1;

		public const int UnregisterplatformaudiouserApiLatest = 1;

		public const int UnregisterplatformuserApiLatest = 1;

		public const int UpdateparticipantvolumeApiLatest = 1;

		public const int UpdatereceivingApiLatest = 1;

		public const int UpdatereceivingvolumeApiLatest = 1;

		public const int UpdatesendingApiLatest = 1;

		public const int UpdatesendingvolumeApiLatest = 1;

		public RTCAudioInterface()
		{
		}

		public RTCAudioInterface(IntPtr innerHandle)
		{
		}

		public ulong AddNotifyAudioBeforeRender(ref AddNotifyAudioBeforeRenderOptions options, object clientData, OnAudioBeforeRenderCallback completionDelegate)
		{
			return 0uL;
		}

		public ulong AddNotifyAudioBeforeSend(ref AddNotifyAudioBeforeSendOptions options, object clientData, OnAudioBeforeSendCallback completionDelegate)
		{
			return 0uL;
		}

		public ulong AddNotifyAudioDevicesChanged(ref AddNotifyAudioDevicesChangedOptions options, object clientData, OnAudioDevicesChangedCallback completionDelegate)
		{
			return 0uL;
		}

		public ulong AddNotifyAudioInputState(ref AddNotifyAudioInputStateOptions options, object clientData, OnAudioInputStateCallback completionDelegate)
		{
			return 0uL;
		}

		public ulong AddNotifyAudioOutputState(ref AddNotifyAudioOutputStateOptions options, object clientData, OnAudioOutputStateCallback completionDelegate)
		{
			return 0uL;
		}

		public ulong AddNotifyParticipantUpdated(ref AddNotifyParticipantUpdatedOptions options, object clientData, OnParticipantUpdatedCallback completionDelegate)
		{
			return 0uL;
		}

		public Result CopyInputDeviceInformationByIndex(ref CopyInputDeviceInformationByIndexOptions options, out InputDeviceInformation? outInputDeviceInformation)
		{
			outInputDeviceInformation = null;
			return Result.Success;
		}

		public Result CopyOutputDeviceInformationByIndex(ref CopyOutputDeviceInformationByIndexOptions options, out OutputDeviceInformation? outOutputDeviceInformation)
		{
			outOutputDeviceInformation = null;
			return Result.Success;
		}

		public AudioInputDeviceInfo? GetAudioInputDeviceByIndex(ref GetAudioInputDeviceByIndexOptions options)
		{
			return null;
		}

		public uint GetAudioInputDevicesCount(ref GetAudioInputDevicesCountOptions options)
		{
			return 0u;
		}

		public AudioOutputDeviceInfo? GetAudioOutputDeviceByIndex(ref GetAudioOutputDeviceByIndexOptions options)
		{
			return null;
		}

		public uint GetAudioOutputDevicesCount(ref GetAudioOutputDevicesCountOptions options)
		{
			return 0u;
		}

		public uint GetInputDevicesCount(ref GetInputDevicesCountOptions options)
		{
			return 0u;
		}

		public uint GetOutputDevicesCount(ref GetOutputDevicesCountOptions options)
		{
			return 0u;
		}

		public void QueryInputDevicesInformation(ref QueryInputDevicesInformationOptions options, object clientData, OnQueryInputDevicesInformationCallback completionDelegate)
		{
		}

		public void QueryOutputDevicesInformation(ref QueryOutputDevicesInformationOptions options, object clientData, OnQueryOutputDevicesInformationCallback completionDelegate)
		{
		}

		public Result RegisterPlatformAudioUser(ref RegisterPlatformAudioUserOptions options)
		{
			return Result.Success;
		}

		public void RegisterPlatformUser(ref RegisterPlatformUserOptions options, object clientData, OnRegisterPlatformUserCallback completionDelegate)
		{
		}

		public void RemoveNotifyAudioBeforeRender(ulong notificationId)
		{
		}

		public void RemoveNotifyAudioBeforeSend(ulong notificationId)
		{
		}

		public void RemoveNotifyAudioDevicesChanged(ulong notificationId)
		{
		}

		public void RemoveNotifyAudioInputState(ulong notificationId)
		{
		}

		public void RemoveNotifyAudioOutputState(ulong notificationId)
		{
		}

		public void RemoveNotifyParticipantUpdated(ulong notificationId)
		{
		}

		public Result SendAudio(ref SendAudioOptions options)
		{
			return Result.Success;
		}

		public Result SetAudioInputSettings(ref SetAudioInputSettingsOptions options)
		{
			return Result.Success;
		}

		public Result SetAudioOutputSettings(ref SetAudioOutputSettingsOptions options)
		{
			return Result.Success;
		}

		public void SetInputDeviceSettings(ref SetInputDeviceSettingsOptions options, object clientData, OnSetInputDeviceSettingsCallback completionDelegate)
		{
		}

		public void SetOutputDeviceSettings(ref SetOutputDeviceSettingsOptions options, object clientData, OnSetOutputDeviceSettingsCallback completionDelegate)
		{
		}

		public Result UnregisterPlatformAudioUser(ref UnregisterPlatformAudioUserOptions options)
		{
			return Result.Success;
		}

		public void UnregisterPlatformUser(ref UnregisterPlatformUserOptions options, object clientData, OnUnregisterPlatformUserCallback completionDelegate)
		{
		}

		public void UpdateParticipantVolume(ref UpdateParticipantVolumeOptions options, object clientData, OnUpdateParticipantVolumeCallback completionDelegate)
		{
		}

		public void UpdateReceiving(ref UpdateReceivingOptions options, object clientData, OnUpdateReceivingCallback completionDelegate)
		{
		}

		public void UpdateReceivingVolume(ref UpdateReceivingVolumeOptions options, object clientData, OnUpdateReceivingVolumeCallback completionDelegate)
		{
		}

		public void UpdateSending(ref UpdateSendingOptions options, object clientData, OnUpdateSendingCallback completionDelegate)
		{
		}

		public void UpdateSendingVolume(ref UpdateSendingVolumeOptions options, object clientData, OnUpdateSendingVolumeCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnAudioBeforeRenderCallbackInternal))]
		internal static void OnAudioBeforeRenderCallbackInternalImplementation(ref AudioBeforeRenderCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnAudioBeforeSendCallbackInternal))]
		internal static void OnAudioBeforeSendCallbackInternalImplementation(ref AudioBeforeSendCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnAudioDevicesChangedCallbackInternal))]
		internal static void OnAudioDevicesChangedCallbackInternalImplementation(ref AudioDevicesChangedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnAudioInputStateCallbackInternal))]
		internal static void OnAudioInputStateCallbackInternalImplementation(ref AudioInputStateCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnAudioOutputStateCallbackInternal))]
		internal static void OnAudioOutputStateCallbackInternalImplementation(ref AudioOutputStateCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnParticipantUpdatedCallbackInternal))]
		internal static void OnParticipantUpdatedCallbackInternalImplementation(ref ParticipantUpdatedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryInputDevicesInformationCallbackInternal))]
		internal static void OnQueryInputDevicesInformationCallbackInternalImplementation(ref OnQueryInputDevicesInformationCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryOutputDevicesInformationCallbackInternal))]
		internal static void OnQueryOutputDevicesInformationCallbackInternalImplementation(ref OnQueryOutputDevicesInformationCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnRegisterPlatformUserCallbackInternal))]
		internal static void OnRegisterPlatformUserCallbackInternalImplementation(ref OnRegisterPlatformUserCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnSetInputDeviceSettingsCallbackInternal))]
		internal static void OnSetInputDeviceSettingsCallbackInternalImplementation(ref OnSetInputDeviceSettingsCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnSetOutputDeviceSettingsCallbackInternal))]
		internal static void OnSetOutputDeviceSettingsCallbackInternalImplementation(ref OnSetOutputDeviceSettingsCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUnregisterPlatformUserCallbackInternal))]
		internal static void OnUnregisterPlatformUserCallbackInternalImplementation(ref OnUnregisterPlatformUserCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUpdateParticipantVolumeCallbackInternal))]
		internal static void OnUpdateParticipantVolumeCallbackInternalImplementation(ref UpdateParticipantVolumeCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUpdateReceivingCallbackInternal))]
		internal static void OnUpdateReceivingCallbackInternalImplementation(ref UpdateReceivingCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUpdateReceivingVolumeCallbackInternal))]
		internal static void OnUpdateReceivingVolumeCallbackInternalImplementation(ref UpdateReceivingVolumeCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUpdateSendingCallbackInternal))]
		internal static void OnUpdateSendingCallbackInternalImplementation(ref UpdateSendingCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUpdateSendingVolumeCallbackInternal))]
		internal static void OnUpdateSendingVolumeCallbackInternalImplementation(ref UpdateSendingVolumeCallbackInfoInternal data)
		{
		}
	}
}
