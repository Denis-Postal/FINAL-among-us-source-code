using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.IntegratedPlatform;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.UI;

namespace Epic.OnlineServices
{
	public static class PS5Bindings
	{
		[PreserveSig]
		internal static extern Result EOS_Initialize(ref PS5InitializeOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_IntegratedPlatformOptionsContainer_Add(IntPtr handle, ref PS5IntegratedPlatformOptionsContainerAddOptionsInternal inOptions);

		[PreserveSig]
		internal static extern ulong EOS_UI_AddNotifyMemoryMonitor(IntPtr handle, ref AddNotifyMemoryMonitorOptionsInternal options, IntPtr clientData, PS5OnMemoryMonitorCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern Result EOS_UI_PrePresent(IntPtr handle, ref PS5PrePresentOptionsInternal options);
	}
}
