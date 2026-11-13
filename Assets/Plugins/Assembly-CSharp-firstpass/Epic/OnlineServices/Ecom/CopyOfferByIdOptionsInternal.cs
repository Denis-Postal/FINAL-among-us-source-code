using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyOfferByIdOptionsInternal : ISettable<CopyOfferByIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_OfferId;

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

		public void Set(ref CopyOfferByIdOptions other)
		{
		}

		public void Set(ref CopyOfferByIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
