using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct QueryOwnershipOptionsInternal : ISettable<QueryOwnershipOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_CatalogItemIds;

		private uint m_CatalogItemIdCount;

		private IntPtr m_CatalogNamespace;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String[] CatalogItemIds
		{
			set
			{
			}
		}

		public Utf8String CatalogNamespace
		{
			set
			{
			}
		}

		public void Set(ref QueryOwnershipOptions other)
		{
		}

		public void Set(ref QueryOwnershipOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
