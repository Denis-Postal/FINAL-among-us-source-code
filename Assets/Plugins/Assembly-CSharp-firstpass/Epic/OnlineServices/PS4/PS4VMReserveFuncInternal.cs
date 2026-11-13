using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PS4
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr PS4VMReserveFuncInternal(UIntPtr sizeInBytes, PS4AccessType accessType, ref IntPtr outContextData);
}
