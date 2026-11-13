using System;

namespace Epic.OnlineServices.XSX
{
	public sealed class XSXInterface
	{
		public static readonly Utf8String XblSettingSessionTemplateName;

		[MonoPInvokeCallback(typeof(XSXVMCommitFuncInternal))]
		internal static int XSXCommitFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(XSXVMDecommitFuncInternal))]
		internal static int XSXDecommitFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(XSXVMReleaseFuncInternal))]
		internal static int XSXReleaseFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(XSXVMReserveFuncInternal))]
		internal static IntPtr XSXReserveFuncInternalImplementation(UIntPtr sizeInBytes, XSXAccessType accessType, ref IntPtr outContextData)
		{
			return (IntPtr)0;
		}
	}
}
