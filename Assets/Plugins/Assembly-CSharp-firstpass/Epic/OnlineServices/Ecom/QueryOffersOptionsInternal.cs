using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct QueryOffersOptionsInternal : ISettable<QueryOffersOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_OverrideCatalogNamespace;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String OverrideCatalogNamespace
		{
			set
			{
			}
		}

		public void Set(ref QueryOffersOptions other)
		{
		}

		public void Set(ref QueryOffersOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
