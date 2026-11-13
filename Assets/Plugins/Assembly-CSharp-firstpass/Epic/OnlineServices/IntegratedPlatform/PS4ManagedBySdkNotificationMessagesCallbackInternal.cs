using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void PS4ManagedBySdkNotificationMessagesCallbackInternal(IntPtr clientData, IntPtr inEvent);
}
