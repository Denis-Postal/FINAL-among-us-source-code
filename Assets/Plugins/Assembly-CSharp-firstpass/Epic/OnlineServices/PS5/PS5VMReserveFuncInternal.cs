using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PS5
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr PS5VMReserveFuncInternal(UIntPtr sizeInBytes, PS5AccessType accessType, ref IntPtr outContextData);
}
