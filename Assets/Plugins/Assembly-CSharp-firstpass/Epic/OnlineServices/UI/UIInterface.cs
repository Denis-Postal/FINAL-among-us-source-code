using System;

namespace Epic.OnlineServices.UI
{
	public sealed class UIInterface : Handle
	{
		public const int Ps4MemorymonitorcallbackinfosystemmemorymonitorreportApiLatest = 1;

		public const int Ps4PrepresentoptionsplatformspecificdataApiLatest = 4;

		public const int Ps4SubmitgraphicscommandscallbackinfoApiLatest = 2;

		public const int Ps5MemorymonitorcallbackinfosystemmemorymonitorreportApiLatest = 1;

		public const int Ps5PrepresentoptionsplatformspecificdataApiLatest = 4;

		public const int Ps5SubmitgraphicscommandscallbackinfoApiLatest = 1;

		public const int AcknowledgecorrelationidApiLatest = 1;

		public const int AcknowledgeeventidApiLatest = 1;

		public const int AddnotifydisplaysettingsupdatedApiLatest = 1;

		public const int AddnotifymemorymonitorApiLatest = 1;

		public const int AddnotifymemorymonitoroptionsApiLatest = 1;

		public const int EventidInvalid = 0;

		public const int GetfriendsexclusiveinputApiLatest = 1;

		public const int GetfriendsvisibleApiLatest = 1;

		public const int GettogglefriendsbuttonApiLatest = 1;

		public const int GettogglefriendskeyApiLatest = 1;

		public const int HidefriendsApiLatest = 1;

		public const int IssocialoverlaypausedApiLatest = 1;

		public const int MemorymonitorcallbackinfoApiLatest = 1;

		public const int PausesocialoverlayApiLatest = 1;

		public const int PrepresentApiLatest = 1;

		public const int RectApiLatest = 1;

		public const int ReportinputstateApiLatest = 2;

		public const int SetdisplaypreferenceApiLatest = 1;

		public const int SettogglefriendsbuttonApiLatest = 1;

		public const int SettogglefriendskeyApiLatest = 1;

		public const int ShowblockplayerApiLatest = 1;

		public const int ShowfriendsApiLatest = 1;

		public const int ShownativeprofileApiLatest = 1;

		public const int ShowreportplayerApiLatest = 1;

		public const int XboxonegdkMemorymonitorcallbackinfosystemmemorymonitorreportApiLatest = 1;

		public const int XboxonegdkPrepresentadditionaldataApiLatest = 1;

		public const int XboxonegdkPrepresentoptionsplatformspecificdataApiLatest = 5;

		public const int XboxonegdkSubmitgraphicscommandscallbackinfoApiLatest = 2;

		public const int XsxMemorymonitorcallbackinfosystemmemorymonitorreportApiLatest = 1;

		public const int XsxPrepresentadditionaldataApiLatest = 1;

		public const int XsxPrepresentoptionsplatformspecificdataApiLatest = 5;

		public const int XsxSubmitgraphicscommandscallbackinfoApiLatest = 2;

		public ulong AddNotifyMemoryMonitor(ref AddNotifyMemoryMonitorOptions options, object clientData, PS4OnMemoryMonitorCallback notificationFn)
		{
			return 0uL;
		}

		public Result PrePresent(ref PS4PrePresentOptions options)
		{
			return Result.Success;
		}

		[MonoPInvokeCallback(typeof(PS4OnMemoryMonitorCallbackInternal))]
		internal static void PS4OnMemoryMonitorCallbackInternalImplementation(ref PS4MemoryMonitorCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(PS4SubmitGraphicsCommandsCallbackInternal))]
		internal static void PS4SubmitGraphicsCommandsCallbackInternalImplementation(ref PS4SubmitGraphicsCommandsCallbackInfoInternal data)
		{
		}

		public ulong AddNotifyMemoryMonitor(ref AddNotifyMemoryMonitorOptions options, object clientData, PS5OnMemoryMonitorCallback notificationFn)
		{
			return 0uL;
		}

		public Result PrePresent(ref PS5PrePresentOptions options)
		{
			return Result.Success;
		}

		[MonoPInvokeCallback(typeof(PS5OnMemoryMonitorCallbackInternal))]
		internal static void PS5OnMemoryMonitorCallbackInternalImplementation(ref PS5MemoryMonitorCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(PS5SubmitGraphicsCommandsCallbackInternal))]
		internal static void PS5SubmitGraphicsCommandsCallbackInternalImplementation(ref PS5SubmitGraphicsCommandsCallbackInfoInternal data)
		{
		}

		public UIInterface()
		{
		}

		public UIInterface(IntPtr innerHandle)
		{
		}

		public Result AcknowledgeEventId(ref AcknowledgeEventIdOptions options)
		{
			return Result.Success;
		}

		public ulong AddNotifyDisplaySettingsUpdated(ref AddNotifyDisplaySettingsUpdatedOptions options, object clientData, OnDisplaySettingsUpdatedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyMemoryMonitor(ref AddNotifyMemoryMonitorOptions options, object clientData, OnMemoryMonitorCallback notificationFn)
		{
			return 0uL;
		}

		public bool GetFriendsExclusiveInput(ref GetFriendsExclusiveInputOptions options)
		{
			return false;
		}

		public bool GetFriendsVisible(ref GetFriendsVisibleOptions options)
		{
			return false;
		}

		public NotificationLocation GetNotificationLocationPreference()
		{
			return NotificationLocation.TopLeft;
		}

		public InputStateButtonFlags GetToggleFriendsButton(ref GetToggleFriendsButtonOptions options)
		{
			return InputStateButtonFlags.None;
		}

		public KeyCombination GetToggleFriendsKey(ref GetToggleFriendsKeyOptions options)
		{
			return KeyCombination.None;
		}

		public void HideFriends(ref HideFriendsOptions options, object clientData, OnHideFriendsCallback completionDelegate)
		{
		}

		public bool IsSocialOverlayPaused(ref IsSocialOverlayPausedOptions options)
		{
			return false;
		}

		public bool IsValidButtonCombination(InputStateButtonFlags buttonCombination)
		{
			return false;
		}

		public bool IsValidKeyCombination(KeyCombination keyCombination)
		{
			return false;
		}

		public Result PauseSocialOverlay(ref PauseSocialOverlayOptions options)
		{
			return Result.Success;
		}

		public Result PrePresent(ref PrePresentOptions options)
		{
			return Result.Success;
		}

		public void RemoveNotifyDisplaySettingsUpdated(ulong id)
		{
		}

		public void RemoveNotifyMemoryMonitor(ulong id)
		{
		}

		public Result ReportInputState(ref ReportInputStateOptions options)
		{
			return Result.Success;
		}

		public Result SetDisplayPreference(ref SetDisplayPreferenceOptions options)
		{
			return Result.Success;
		}

		public Result SetToggleFriendsButton(ref SetToggleFriendsButtonOptions options)
		{
			return Result.Success;
		}

		public Result SetToggleFriendsKey(ref SetToggleFriendsKeyOptions options)
		{
			return Result.Success;
		}

		public void ShowBlockPlayer(ref ShowBlockPlayerOptions options, object clientData, OnShowBlockPlayerCallback completionDelegate)
		{
		}

		public void ShowFriends(ref ShowFriendsOptions options, object clientData, OnShowFriendsCallback completionDelegate)
		{
		}

		public void ShowNativeProfile(ref ShowNativeProfileOptions options, object clientData, OnShowNativeProfileCallback completionDelegate)
		{
		}

		public void ShowReportPlayer(ref ShowReportPlayerOptions options, object clientData, OnShowReportPlayerCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnDisplaySettingsUpdatedCallbackInternal))]
		internal static void OnDisplaySettingsUpdatedCallbackInternalImplementation(ref OnDisplaySettingsUpdatedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnHideFriendsCallbackInternal))]
		internal static void OnHideFriendsCallbackInternalImplementation(ref HideFriendsCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnMemoryMonitorCallbackInternal))]
		internal static void OnMemoryMonitorCallbackInternalImplementation(ref MemoryMonitorCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnShowBlockPlayerCallbackInternal))]
		internal static void OnShowBlockPlayerCallbackInternalImplementation(ref OnShowBlockPlayerCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnShowFriendsCallbackInternal))]
		internal static void OnShowFriendsCallbackInternalImplementation(ref ShowFriendsCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnShowNativeProfileCallbackInternal))]
		internal static void OnShowNativeProfileCallbackInternalImplementation(ref ShowNativeProfileCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnShowReportPlayerCallbackInternal))]
		internal static void OnShowReportPlayerCallbackInternalImplementation(ref OnShowReportPlayerCallbackInfoInternal data)
		{
		}

		public ulong AddNotifyMemoryMonitor(ref AddNotifyMemoryMonitorOptions options, object clientData, XboxOneGDKOnMemoryMonitorCallback notificationFn)
		{
			return 0uL;
		}

		public Result PrePresent(ref XboxOneGDKPrePresentOptions options)
		{
			return Result.Success;
		}

		[MonoPInvokeCallback(typeof(XboxOneGDKOnMemoryMonitorCallbackInternal))]
		internal static void XboxOneGDKOnMemoryMonitorCallbackInternalImplementation(ref XboxOneGDKMemoryMonitorCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(XboxOneGDKSubmitGraphicsCommandsCallbackInternal))]
		internal static void XboxOneGDKSubmitGraphicsCommandsCallbackInternalImplementation(ref XboxOneGDKSubmitGraphicsCommandsCallbackInfoInternal data)
		{
		}

		public ulong AddNotifyMemoryMonitor(ref AddNotifyMemoryMonitorOptions options, object clientData, XSXOnMemoryMonitorCallback notificationFn)
		{
			return 0uL;
		}

		public Result PrePresent(ref XSXPrePresentOptions options)
		{
			return Result.Success;
		}

		[MonoPInvokeCallback(typeof(XSXOnMemoryMonitorCallbackInternal))]
		internal static void XSXOnMemoryMonitorCallbackInternalImplementation(ref XSXMemoryMonitorCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(XSXSubmitGraphicsCommandsCallbackInternal))]
		internal static void XSXSubmitGraphicsCommandsCallbackInternalImplementation(ref XSXSubmitGraphicsCommandsCallbackInfoInternal data)
		{
		}
	}
}
