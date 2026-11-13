using System;

namespace Epic.OnlineServices.XSX
{
	public delegate IntPtr XSXVMReserveFunc(UIntPtr sizeInBytes, XSXAccessType accessType, out IntPtr outContextData);
}
