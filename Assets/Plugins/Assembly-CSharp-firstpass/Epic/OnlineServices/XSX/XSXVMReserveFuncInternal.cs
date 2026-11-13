using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.XSX
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr XSXVMReserveFuncInternal(UIntPtr sizeInBytes, XSXAccessType accessType, ref IntPtr outContextData);
}
