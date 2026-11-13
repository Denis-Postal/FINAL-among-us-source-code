using System;

namespace Epic.OnlineServices.PS5
{
	public sealed class PS5Interface
	{
		[MonoPInvokeCallback(typeof(PS5VMCommitFuncInternal))]
		internal static int PS5CommitFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(PS5VMDecommitFuncInternal))]
		internal static int PS5DecommitFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(PS5VMReleaseFuncInternal))]
		internal static int PS5ReleaseFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(PS5VMReserveFuncInternal))]
		internal static IntPtr PS5ReserveFuncInternalImplementation(UIntPtr sizeInBytes, PS5AccessType accessType, ref IntPtr outContextData)
		{
			return (IntPtr)0;
		}
	}
}
