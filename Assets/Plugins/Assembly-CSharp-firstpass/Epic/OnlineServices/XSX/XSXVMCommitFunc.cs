using System;

namespace Epic.OnlineServices.XSX
{
	public delegate bool XSXVMCommitFunc(IntPtr address, UIntPtr sizeInBytes, IntPtr contextData);
}
