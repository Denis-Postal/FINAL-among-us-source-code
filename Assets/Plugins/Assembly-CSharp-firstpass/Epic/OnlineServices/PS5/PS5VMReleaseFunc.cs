using System;

namespace Epic.OnlineServices.PS5
{
	public delegate bool PS5VMReleaseFunc(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
