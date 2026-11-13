using System;

namespace Epic.OnlineServices.Switch
{
	public delegate bool SwitchVMDecommitFunc(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
