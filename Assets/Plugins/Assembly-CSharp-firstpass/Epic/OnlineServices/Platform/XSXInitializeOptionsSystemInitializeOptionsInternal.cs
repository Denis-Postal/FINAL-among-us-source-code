using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.XSX;

namespace Epic.OnlineServices.Platform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 64)]
	internal struct XSXInitializeOptionsSystemInitializeOptionsInternal : IGettable<XSXInitializeOptionsSystemInitializeOptions>, ISettable<XSXInitializeOptionsSystemInitializeOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_OverlayDllBasePath;

		private IntPtr m_Reserved;

		private IntPtr m_ReserveVirtualMemoryFunction;

		private IntPtr m_ReleaseVirtualMemoryFunction;

		private IntPtr m_CommitVirtualMemoryFunction;

		private IntPtr m_DecommitVirtualMemoryFunction;

		private uint m_PlatformSDKVersion;

		private static XSXVMReserveFuncInternal s_ReserveVirtualMemoryFunction;

		private static XSXVMReleaseFuncInternal s_ReleaseVirtualMemoryFunction;

		private static XSXVMCommitFuncInternal s_CommitVirtualMemoryFunction;

		private static XSXVMDecommitFuncInternal s_DecommitVirtualMemoryFunction;

		public Utf8String OverlayDllBasePath
		{
			get
			{
				return null;
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

		public static XSXVMReserveFuncInternal ReserveVirtualMemoryFunction => null;

		public static XSXVMReleaseFuncInternal ReleaseVirtualMemoryFunction => null;

		public static XSXVMCommitFuncInternal CommitVirtualMemoryFunction => null;

		public static XSXVMDecommitFuncInternal DecommitVirtualMemoryFunction => null;

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

		public void Set(ref XSXInitializeOptionsSystemInitializeOptions other)
		{
		}

		public void Set(ref XSXInitializeOptionsSystemInitializeOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XSXInitializeOptionsSystemInitializeOptions output)
		{
			output = default;
		}
	}
}
