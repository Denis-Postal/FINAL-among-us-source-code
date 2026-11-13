using System;

namespace Epic.OnlineServices.PS5
{
	public delegate IntPtr PS5VMReserveFunc(UIntPtr sizeInBytes, PS5AccessType accessType, out IntPtr outContextData);
}
