using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct CheckoutOptionsInternal : ISettable<CheckoutOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_OverrideCatalogNamespace;

		private uint m_EntryCount;

		private IntPtr m_Entries;

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

		public CheckoutEntry[] Entries
		{
			set
			{
			}
		}

		public void Set(ref CheckoutOptions other)
		{
		}

		public void Set(ref CheckoutOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
