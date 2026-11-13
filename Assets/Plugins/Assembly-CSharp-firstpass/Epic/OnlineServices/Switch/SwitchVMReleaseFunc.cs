using System;

namespace Epic.OnlineServices.Switch
{
	public delegate bool SwitchVMReleaseFunc(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
