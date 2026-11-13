using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct CopyOfferItemByIndexOptionsInternal : ISettable<CopyOfferItemByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_OfferId;

		private uint m_ItemIndex;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String OfferId
		{
			set
			{
			}
		}

		public uint ItemIndex
		{
			set
			{
			}
		}

		public void Set(ref CopyOfferItemByIndexOptions other)
		{
		}

		public void Set(ref CopyOfferItemByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
