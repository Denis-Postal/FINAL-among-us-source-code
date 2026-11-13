using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct QueryProductUserIdMappingsOptionsInternal : ISettable<QueryProductUserIdMappingsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private ExternalAccountType m_AccountIdType_DEPRECATED;

		private IntPtr m_ProductUserIds;

		private uint m_ProductUserIdCount;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public ExternalAccountType AccountIdType_DEPRECATED
		{
			set
			{
			}
		}

		public ProductUserId[] ProductUserIds
		{
			set
			{
			}
		}

		public void Set(ref QueryProductUserIdMappingsOptions other)
		{
		}

		public void Set(ref QueryProductUserIdMappingsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
