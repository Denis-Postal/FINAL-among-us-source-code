using System;

namespace Epic.OnlineServices.PS5
{
	public delegate bool PS5VMDecommitFunc(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
