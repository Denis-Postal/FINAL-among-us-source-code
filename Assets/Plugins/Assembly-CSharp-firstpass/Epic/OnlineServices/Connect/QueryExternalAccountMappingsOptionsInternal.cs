using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct QueryExternalAccountMappingsOptionsInternal : ISettable<QueryExternalAccountMappingsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private ExternalAccountType m_AccountIdType;

		private IntPtr m_ExternalAccountIds;

		private uint m_ExternalAccountIdCount;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public ExternalAccountType AccountIdType
		{
			set
			{
			}
		}

		public Utf8String[] ExternalAccountIds
		{
			set
			{
			}
		}

		public void Set(ref QueryExternalAccountMappingsOptions other)
		{
		}

		public void Set(ref QueryExternalAccountMappingsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
