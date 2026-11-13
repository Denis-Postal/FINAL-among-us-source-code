using System;

namespace Epic.OnlineServices.PS5
{
	public delegate bool PS5VMCommitFunc(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
