using System;
using Epic.OnlineServices.PS5;

namespace Epic.OnlineServices.Platform
{
	public struct PS5InitializeOptionsSystemInitializeOptions
	{
		public Utf8String OverlayPrxBasePath { get; set; }

		internal IntPtr AllocatePhysicalMemoryFunction_DEPRECATED { get; set; }

		internal IntPtr ReleasePhysicalMemoryFunction_DEPRECATED { get; set; }

		public IntPtr Reserved { get; set; }

		public PS5VMReserveFunc ReserveVirtualMemoryFunction { get; set; }

		public PS5VMReleaseFunc ReleaseVirtualMemoryFunction { get; set; }

		public PS5VMCommitFunc CommitVirtualMemoryFunction { get; set; }

		public PS5VMDecommitFunc DecommitVirtualMemoryFunction { get; set; }

		public uint PlatformSDKVersion { get; set; }

		internal void Set(ref PS5InitializeOptionsSystemInitializeOptionsInternal other)
		{
		}
	}
}
