using System;
using Epic.OnlineServices.XboxOneGDK;

namespace Epic.OnlineServices.Platform
{
	public struct XboxOneGDKInitializeOptionsSystemInitializeOptions
	{
		public Utf8String OverlayDllBasePath { get; set; }

		public IntPtr Reserved { get; set; }

		public XboxOneGDKVMReserveFunc ReserveVirtualMemoryFunction { get; set; }

		public XboxOneGDKVMReleaseFunc ReleaseVirtualMemoryFunction { get; set; }

		public XboxOneGDKVMCommitFunc CommitVirtualMemoryFunction { get; set; }

		public XboxOneGDKVMDecommitFunc DecommitVirtualMemoryFunction { get; set; }

		public uint PlatformSDKVersion { get; set; }

		internal void Set(ref XboxOneGDKInitializeOptionsSystemInitializeOptionsInternal other)
		{
		}
	}
}
