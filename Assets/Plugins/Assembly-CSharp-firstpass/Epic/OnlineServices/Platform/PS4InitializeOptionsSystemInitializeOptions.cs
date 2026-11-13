using System;
using Epic.OnlineServices.PS4;

namespace Epic.OnlineServices.Platform
{
	public struct PS4InitializeOptionsSystemInitializeOptions
	{
		internal Utf8String CommandLine_DEPRECATED { get; set; }

		public Utf8String OverlayPrxBasePath { get; set; }

		internal IntPtr AllocatePhysicalMemoryFunction_DEPRECATED { get; set; }

		internal IntPtr ReleasePhysicalMemoryFunction_DEPRECATED { get; set; }

		public IntPtr Reserved { get; set; }

		public PS4VMReserveFunc ReserveVirtualMemoryFunction { get; set; }

		public PS4VMReleaseFunc ReleaseVirtualMemoryFunction { get; set; }

		public PS4VMCommitFunc CommitVirtualMemoryFunction { get; set; }

		public PS4VMDecommitFunc DecommitVirtualMemoryFunction { get; set; }

		public uint PlatformSDKVersion { get; set; }

		internal void Set(ref PS4InitializeOptionsSystemInitializeOptionsInternal other)
		{
		}
	}
}
