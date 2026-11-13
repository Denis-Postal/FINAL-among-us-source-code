using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.PS4;

namespace Epic.OnlineServices.Platform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 88)]
	internal struct PS4InitializeOptionsSystemInitializeOptionsInternal : IGettable<PS4InitializeOptionsSystemInitializeOptions>, ISettable<PS4InitializeOptionsSystemInitializeOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_CommandLine_DEPRECATED;

		private IntPtr m_OverlayPrxBasePath;

		private IntPtr m_AllocatePhysicalMemoryFunction_DEPRECATED;

		private IntPtr m_ReleasePhysicalMemoryFunction_DEPRECATED;

		private IntPtr m_Reserved;

		private IntPtr m_ReserveVirtualMemoryFunction;

		private IntPtr m_ReleaseVirtualMemoryFunction;

		private IntPtr m_CommitVirtualMemoryFunction;

		private IntPtr m_DecommitVirtualMemoryFunction;

		private uint m_PlatformSDKVersion;

		private static PS4VMReserveFuncInternal s_ReserveVirtualMemoryFunction;

		private static PS4VMReleaseFuncInternal s_ReleaseVirtualMemoryFunction;

		private static PS4VMCommitFuncInternal s_CommitVirtualMemoryFunction;

		private static PS4VMDecommitFuncInternal s_DecommitVirtualMemoryFunction;

		public Utf8String CommandLine_DEPRECATED
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String OverlayPrxBasePath
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public IntPtr AllocatePhysicalMemoryFunction_DEPRECATED
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		public IntPtr ReleasePhysicalMemoryFunction_DEPRECATED
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		public IntPtr Reserved
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		public static PS4VMReserveFuncInternal ReserveVirtualMemoryFunction => null;

		public static PS4VMReleaseFuncInternal ReleaseVirtualMemoryFunction => null;

		public static PS4VMCommitFuncInternal CommitVirtualMemoryFunction => null;

		public static PS4VMDecommitFuncInternal DecommitVirtualMemoryFunction => null;

		public uint PlatformSDKVersion
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public void Set(ref PS4InitializeOptionsSystemInitializeOptions other)
		{
		}

		public void Set(ref PS4InitializeOptionsSystemInitializeOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PS4InitializeOptionsSystemInitializeOptions output)
		{
			output = default;
		}
	}
}
