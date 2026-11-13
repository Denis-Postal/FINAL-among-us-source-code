using System;

namespace Epic.OnlineServices.Platform
{
	public struct XboxOneGDKInitializeOptions
	{
		public IntPtr AllocateMemoryFunction { get; set; }

		public IntPtr ReallocateMemoryFunction { get; set; }

		public IntPtr ReleaseMemoryFunction { get; set; }

		public Utf8String ProductName { get; set; }

		public Utf8String ProductVersion { get; set; }

		public IntPtr Reserved { get; set; }

		public XboxOneGDKInitializeOptionsSystemInitializeOptions? SystemInitializeOptions { get; set; }

		public InitializeThreadAffinity? OverrideThreadAffinity { get; set; }
	}
}
