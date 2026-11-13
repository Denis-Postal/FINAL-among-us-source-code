using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PS5
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int PS5VMDecommitFuncInternal(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
