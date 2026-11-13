using System;

namespace Epic.OnlineServices.PS4
{
	public delegate bool PS4VMCommitFunc(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
