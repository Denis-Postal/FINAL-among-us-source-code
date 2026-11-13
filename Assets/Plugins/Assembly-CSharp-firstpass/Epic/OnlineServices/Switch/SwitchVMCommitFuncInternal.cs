using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Switch
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int SwitchVMCommitFuncInternal(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
