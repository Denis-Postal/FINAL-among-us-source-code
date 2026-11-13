using System;

namespace Epic.OnlineServices.PS4
{
	public delegate IntPtr PS4VMReserveFunc(UIntPtr sizeInBytes, PS4AccessType accessType, out IntPtr outContextData);
}
