using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.IntegratedPlatform;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.UI;

namespace Epic.OnlineServices
{
	public static class PS4Bindings
	{
		[PreserveSig]
		internal static extern Result EOS_Initialize(ref PS4InitializeOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_IntegratedPlatformOptionsContainer_Add(IntPtr handle, ref PS4IntegratedPlatformOptionsContainerAddOptionsInternal inOptions);

		[PreserveSig]
		internal static extern Result EOS_Platform_PS4_ManagedByApplicationNotificationMessages(IntPtr platformHandle, IntPtr inEvent);

		[PreserveSig]
		internal static extern ulong EOS_UI_AddNotifyMemoryMonitor(IntPtr handle, ref AddNotifyMemoryMonitorOptionsInternal options, IntPtr clientData, PS4OnMemoryMonitorCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern Result EOS_UI_PrePresent(IntPtr handle, ref PS4PrePresentOptionsInternal options);
	}
}
