using System;
using Epic.OnlineServices.XSX;

namespace Epic.OnlineServices.Platform
{
	public struct XSXInitializeOptionsSystemInitializeOptions
	{
		public Utf8String OverlayDllBasePath { get; set; }

		public IntPtr Reserved { get; set; }

		public XSXVMReserveFunc ReserveVirtualMemoryFunction { get; set; }

		public XSXVMReleaseFunc ReleaseVirtualMemoryFunction { get; set; }

		public XSXVMCommitFunc CommitVirtualMemoryFunction { get; set; }

		public XSXVMDecommitFunc DecommitVirtualMemoryFunction { get; set; }

		public uint PlatformSDKVersion { get; set; }

		internal void Set(ref XSXInitializeOptionsSystemInitializeOptionsInternal other)
		{
		}
	}
}
