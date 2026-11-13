using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct CatalogReleaseInternal : IGettable<CatalogRelease>, ISettable<CatalogRelease>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_CompatibleAppIdCount;

		private IntPtr m_CompatibleAppIds;

		private uint m_CompatiblePlatformCount;

		private IntPtr m_CompatiblePlatforms;

		private IntPtr m_ReleaseNote;

		public Utf8String[] CompatibleAppIds
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String[] CompatiblePlatforms
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String ReleaseNote
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref CatalogRelease other)
		{
		}

		public void Set(ref CatalogRelease? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out CatalogRelease output)
		{
			output = default;
		}
	}
}
