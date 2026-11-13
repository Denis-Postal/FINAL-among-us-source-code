using System;

namespace Epic.OnlineServices.PS4
{
	public sealed class PS4Interface
	{
		[MonoPInvokeCallback(typeof(PS4VMCommitFuncInternal))]
		internal static int PS4CommitFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(PS4VMDecommitFuncInternal))]
		internal static int PS4DecommitFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(PS4VMReleaseFuncInternal))]
		internal static int PS4ReleaseFuncInternalImplementation(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData)
		{
			return 0;
		}

		[MonoPInvokeCallback(typeof(PS4VMReserveFuncInternal))]
		internal static IntPtr PS4ReserveFuncInternalImplementation(UIntPtr sizeInBytes, PS4AccessType accessType, ref IntPtr outContextData)
		{
			return (IntPtr)0;
		}
	}
}
