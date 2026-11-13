using System;

namespace Epic.OnlineServices.PS4
{
	public delegate bool PS4VMReleaseFunc(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
