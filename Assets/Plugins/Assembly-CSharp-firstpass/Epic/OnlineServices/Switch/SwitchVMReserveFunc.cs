using System;

namespace Epic.OnlineServices.Switch
{
	public delegate IntPtr SwitchVMReserveFunc(UIntPtr sizeInBytes, SwitchAccessType accessType, out IntPtr outContextData);
}
