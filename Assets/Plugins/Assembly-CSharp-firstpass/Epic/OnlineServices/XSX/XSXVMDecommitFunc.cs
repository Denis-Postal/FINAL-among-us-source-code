using System;

namespace Epic.OnlineServices.XSX
{
	public delegate bool XSXVMDecommitFunc(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
