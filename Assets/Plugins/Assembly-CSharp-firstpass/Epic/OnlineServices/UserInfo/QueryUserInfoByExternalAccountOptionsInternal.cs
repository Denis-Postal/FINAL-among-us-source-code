using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct QueryUserInfoByExternalAccountOptionsInternal : ISettable<QueryUserInfoByExternalAccountOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_ExternalAccountId;

		private ExternalAccountType m_AccountType;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String ExternalAccountId
		{
			set
			{
			}
		}

		public ExternalAccountType AccountType
		{
			set
			{
			}
		}

		public void Set(ref QueryUserInfoByExternalAccountOptions other)
		{
		}

		public void Set(ref QueryUserInfoByExternalAccountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
