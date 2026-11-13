using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyFileMetadataAtIndexOptionsInternal : ISettable<CopyFileMetadataAtIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private uint m_Index;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public uint Index
		{
			set
			{
			}
		}

		public void Set(ref CopyFileMetadataAtIndexOptions other)
		{
		}

		public void Set(ref CopyFileMetadataAtIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
