using System;

namespace Epic.OnlineServices.XboxOneGDK
{
	public sealed class XboxOneGDKInterface
	{
		public static readonly Utf8String XblSettingSessionTemplateName;

		[MonoPInvokeCallback(typeof(XboxOneGDKVMCommitFuncInternal))]
		internal static int XboxOneGDKCommitFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(XboxOneGDKVMDecommitFuncInternal))]
		internal static int XboxOneGDKDecommitFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(XboxOneGDKVMReleaseFuncInternal))]
		internal static int XboxOneGDKReleaseFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(XboxOneGDKVMReserveFuncInternal))]
		internal static IntPtr XboxOneGDKReserveFuncInternalImplementation(UIntPtr sizeInBytes, XboxOneGDKAccessType accessType, ref IntPtr outContextData)
		{
			return (IntPtr)0;
		}
	}
}
