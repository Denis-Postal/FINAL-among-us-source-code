using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PS4
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int PS4VMDecommitFuncInternal(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
