using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.XboxOneGDK;

namespace Epic.OnlineServices.Platform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 64)]
	internal struct XboxOneGDKInitializeOptionsSystemInitializeOptionsInternal : IGettable<XboxOneGDKInitializeOptionsSystemInitializeOptions>, ISettable<XboxOneGDKInitializeOptionsSystemInitializeOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_OverlayDllBasePath;

		private IntPtr m_Reserved;

		private IntPtr m_ReserveVirtualMemoryFunction;

		private IntPtr m_ReleaseVirtualMemoryFunction;

		private IntPtr m_CommitVirtualMemoryFunction;

		private IntPtr m_DecommitVirtualMemoryFunction;

		private uint m_PlatformSDKVersion;

		private static XboxOneGDKVMReserveFuncInternal s_ReserveVirtualMemoryFunction;

		private static XboxOneGDKVMReleaseFuncInternal s_ReleaseVirtualMemoryFunction;

		private static XboxOneGDKVMCommitFuncInternal s_CommitVirtualMemoryFunction;

		private static XboxOneGDKVMDecommitFuncInternal s_DecommitVirtualMemoryFunction;

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

		public static XboxOneGDKVMReserveFuncInternal ReserveVirtualMemoryFunction => null;

		public static XboxOneGDKVMReleaseFuncInternal ReleaseVirtualMemoryFunction => null;

		public static XboxOneGDKVMCommitFuncInternal CommitVirtualMemoryFunction => null;

		public static XboxOneGDKVMDecommitFuncInternal DecommitVirtualMemoryFunction => null;

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

		public void Set(ref XboxOneGDKInitializeOptionsSystemInitializeOptions other)
		{
		}

		public void Set(ref XboxOneGDKInitializeOptionsSystemInitializeOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XboxOneGDKInitializeOptionsSystemInitializeOptions output)
		{
			output = default;
		}
	}
}
