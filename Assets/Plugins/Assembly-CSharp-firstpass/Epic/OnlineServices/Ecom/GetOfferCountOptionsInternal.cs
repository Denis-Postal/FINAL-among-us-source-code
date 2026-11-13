using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct GetOfferCountOptionsInternal : ISettable<GetOfferCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref GetOfferCountOptions other)
		{
		}

		public void Set(ref GetOfferCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
