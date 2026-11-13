using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.XSX
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int XSXVMReleaseFuncInternal(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
