using System;

namespace Epic.OnlineServices.XboxOneGDK
{
	public delegate IntPtr XboxOneGDKVMReserveFunc(UIntPtr sizeInBytes, XboxOneGDKAccessType accessType, out IntPtr outContextData);
}
