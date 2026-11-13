using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct CopyItemReleaseByIndexOptionsInternal : ISettable<CopyItemReleaseByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_ItemId;

		private uint m_ReleaseIndex;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String ItemId
		{
			set
			{
			}
		}

		public uint ReleaseIndex
		{
			set
			{
			}
		}

		public void Set(ref CopyItemReleaseByIndexOptions other)
		{
		}

		public void Set(ref CopyItemReleaseByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
