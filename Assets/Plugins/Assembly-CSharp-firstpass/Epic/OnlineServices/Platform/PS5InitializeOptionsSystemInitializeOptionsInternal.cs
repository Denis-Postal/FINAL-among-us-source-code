using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.PS5;

namespace Epic.OnlineServices.Platform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 80)]
	internal struct PS5InitializeOptionsSystemInitializeOptionsInternal : IGettable<PS5InitializeOptionsSystemInitializeOptions>, ISettable<PS5InitializeOptionsSystemInitializeOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_OverlayPrxBasePath;

		private IntPtr m_AllocatePhysicalMemoryFunction_DEPRECATED;

		private IntPtr m_ReleasePhysicalMemoryFunction_DEPRECATED;

		private IntPtr m_Reserved;

		private IntPtr m_ReserveVirtualMemoryFunction;

		private IntPtr m_ReleaseVirtualMemoryFunction;

		private IntPtr m_CommitVirtualMemoryFunction;

		private IntPtr m_DecommitVirtualMemoryFunction;

		private uint m_PlatformSDKVersion;

		private static PS5VMReserveFuncInternal s_ReserveVirtualMemoryFunction;

		private static PS5VMReleaseFuncInternal s_ReleaseVirtualMemoryFunction;

		private static PS5VMCommitFuncInternal s_CommitVirtualMemoryFunction;

		private static PS5VMDecommitFuncInternal s_DecommitVirtualMemoryFunction;

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

		public static PS5VMReserveFuncInternal ReserveVirtualMemoryFunction => null;

		public static PS5VMReleaseFuncInternal ReleaseVirtualMemoryFunction => null;

		public static PS5VMCommitFuncInternal CommitVirtualMemoryFunction => null;

		public static PS5VMDecommitFuncInternal DecommitVirtualMemoryFunction => null;

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

		public void Set(ref PS5InitializeOptionsSystemInitializeOptions other)
		{
		}

		public void Set(ref PS5InitializeOptionsSystemInitializeOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PS5InitializeOptionsSystemInitializeOptions output)
		{
			output = default;
		}
	}
}
