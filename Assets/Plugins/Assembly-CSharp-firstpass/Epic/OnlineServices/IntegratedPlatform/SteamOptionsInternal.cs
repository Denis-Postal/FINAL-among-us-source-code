using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct SteamOptionsInternal : IGettable<SteamOptions>, ISettable<SteamOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_OverrideLibraryPath;

		private uint m_SteamMajorVersion;

		private uint m_SteamMinorVersion;

		private IntPtr m_SteamApiInterfaceVersionsArray;

		private uint m_SteamApiInterfaceVersionsArrayBytes;

		public Utf8String OverrideLibraryPath
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public uint SteamMajorVersion
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public uint SteamMinorVersion
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public Utf8String SteamApiInterfaceVersionsArray
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public uint SteamApiInterfaceVersionsArrayBytes
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public void Set(ref SteamOptions other)
		{
		}

		public void Set(ref SteamOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SteamOptions output)
		{
			output = default;
		}
	}
}
