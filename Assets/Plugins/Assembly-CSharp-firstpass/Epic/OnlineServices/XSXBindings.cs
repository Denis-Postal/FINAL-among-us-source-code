using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.IntegratedPlatform;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.UI;

namespace Epic.OnlineServices
{
	public static class XSXBindings
	{
		[PreserveSig]
		internal static extern Result EOS_Initialize(ref XSXInitializeOptionsInternal options);

		[PreserveSig]
		internal static extern Result EOS_IntegratedPlatformOptionsContainer_Add(IntPtr handle, ref XSXIntegratedPlatformOptionsContainerAddOptionsInternal inOptions);

		[PreserveSig]
		internal static extern IntPtr EOS_Platform_Create(ref XSXOptionsInternal options);

		[PreserveSig]
		internal static extern ulong EOS_UI_AddNotifyMemoryMonitor(IntPtr handle, ref AddNotifyMemoryMonitorOptionsInternal options, IntPtr clientData, XSXOnMemoryMonitorCallbackInternal notificationFn);

		[PreserveSig]
		internal static extern Result EOS_UI_PrePresent(IntPtr handle, ref XSXPrePresentOptionsInternal options);
	}
}
