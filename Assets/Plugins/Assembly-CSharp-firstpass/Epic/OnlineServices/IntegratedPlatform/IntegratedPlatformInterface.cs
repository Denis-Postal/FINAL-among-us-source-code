using System;

namespace Epic.OnlineServices.IntegratedPlatform
{
	public sealed class IntegratedPlatformInterface : Handle
	{
		public const int AddnotifyuserloginstatuschangedApiLatest = 1;

		public const int ClearuserprelogoutcallbackApiLatest = 1;

		public const int CreateintegratedplatformoptionscontainerApiLatest = 1;

		public const int FinalizedeferreduserlogoutApiLatest = 1;

		public const int OptionsApiLatest = 1;

		public const int SetuserloginstatusApiLatest = 1;

		public const int SetuserprelogoutcallbackApiLatest = 1;

		public const int SteamMaxSteamapiinterfaceversionsarraySize = 4096;

		public const int SteamOptionsApiLatest = 3;

		public const int Ps4OptionsinitoptionsApiLatest = 1;

		public const int Ps5OptionsinitoptionsApiLatest = 1;

		public const int SwitchOptionsinitoptionsApiLatest = 1;

		public const int XboxonegdkXbloptionsinitoptionsApiLatest = 2;

		public const int XsxXbloptionsinitoptionsApiLatest = 2;

		public IntegratedPlatformInterface()
		{
		}

		public IntegratedPlatformInterface(IntPtr innerHandle)
		{
		}

		public ulong AddNotifyUserLoginStatusChanged(ref AddNotifyUserLoginStatusChangedOptions options, object clientData, OnUserLoginStatusChangedCallback callbackFunction)
		{
			return 0uL;
		}

		public void ClearUserPreLogoutCallback(ref ClearUserPreLogoutCallbackOptions options)
		{
		}

		public static Result CreateIntegratedPlatformOptionsContainer(ref CreateIntegratedPlatformOptionsContainerOptions options, out IntegratedPlatformOptionsContainer outIntegratedPlatformOptionsContainerHandle)
		{
			outIntegratedPlatformOptionsContainerHandle = null;
			return Result.Success;
		}

		public Result FinalizeDeferredUserLogout(ref FinalizeDeferredUserLogoutOptions options)
		{
			return Result.Success;
		}

		public void RemoveNotifyUserLoginStatusChanged(ulong notificationId)
		{
		}

		public Result SetUserLoginStatus(ref SetUserLoginStatusOptions options)
		{
			return Result.Success;
		}

		public Result SetUserPreLogoutCallback(ref SetUserPreLogoutCallbackOptions options, object clientData, OnUserPreLogoutCallback callbackFunction)
		{
			return Result.Success;
		}

		[MonoPInvokeCallback(typeof(OnUserLoginStatusChangedCallbackInternal))]
		internal static void OnUserLoginStatusChangedCallbackInternalImplementation(ref UserLoginStatusChangedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUserPreLogoutCallbackInternal))]
		internal static IntegratedPlatformPreLogoutAction OnUserPreLogoutCallbackInternalImplementation(ref UserPreLogoutCallbackInfoInternal data)
		{
			return IntegratedPlatformPreLogoutAction.ProcessLogoutImmediately;
		}

		[MonoPInvokeCallback(typeof(PS4ManagedBySdkNotificationMessagesCallbackInternal))]
		internal static void PS4ManagedBySdkNotificationMessagesCallbackInternalImplementation(IntPtr clientData, IntPtr inEvent)
		{
		}
	}
}
