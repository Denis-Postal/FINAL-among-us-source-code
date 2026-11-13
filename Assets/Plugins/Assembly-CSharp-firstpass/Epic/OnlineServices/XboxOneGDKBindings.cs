using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.IntegratedPlatform;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.UI;

namespace Epic.OnlineServices
{
	public static class XboxOneGDKBindings
	{
		[PreserveSig]
		internal static extern Result EOS_Initialize(ref XboxOneGDKInitializeOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_IntegratedPlatformOptionsContainer_Add(IntPtr handle, ref XboxOneGDKIntegratedPlatformOptionsContainerAddOptionsInternal inOptions);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_Create(ref XboxOneGDKOptionsInternal options);

		[PreserveSig]
		internal static extern ulong EOS_UI_AddNotifyMemoryMonitor(IntPtr handle, ref AddNotifyMemoryMonitorOptionsInternal options, IntPtr clientData, XboxOneGDKOnMemoryMonitorCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern Result EOS_UI_PrePresent(IntPtr handle, ref XboxOneGDKPrePresentOptionsInternal options);
	}
}
