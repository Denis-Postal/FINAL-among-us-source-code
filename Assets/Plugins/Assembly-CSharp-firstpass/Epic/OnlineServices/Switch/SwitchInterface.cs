using System;

namespace Epic.OnlineServices.Switch
{
	public sealed class SwitchInterface
	{
		public const int MinCacheStorageSizeKb = 2048;

		[MonoPInvokeCallback(typeof(SwitchVMCommitFuncInternal))]
		internal static int SwitchCommitFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(SwitchVMDecommitFuncInternal))]
		internal static int SwitchDecommitFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(SwitchOnNetworkRequestedInternal))]
		internal static int SwitchOnNetworkRequestedInternalImplementation()
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(SwitchVMReleaseFuncInternal))]
		internal static int SwitchReleaseFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(SwitchVMReserveFuncInternal))]
		internal static IntPtr SwitchReserveFuncInternalImplementation(UIntPtr sizeInBytes, SwitchAccessType accessType, ref IntPtr outContextData)
		{
			return (IntPtr)0;
		}
	}
}
