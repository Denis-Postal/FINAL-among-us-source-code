using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.XSX
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int XSXVMDecommitFuncInternal(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
