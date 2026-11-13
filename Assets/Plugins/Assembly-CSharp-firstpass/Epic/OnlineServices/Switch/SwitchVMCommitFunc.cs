using System;

namespace Epic.OnlineServices.Switch
{
	public delegate bool SwitchVMCommitFunc(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
