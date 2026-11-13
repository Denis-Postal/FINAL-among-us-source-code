using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct CopyExternalUserInfoByAccountIdOptionsInternal : ISettable<CopyExternalUserInfoByAccountIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

		private IntPtr m_AccountId;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public EpicAccountId TargetUserId
		{
			set
			{
			}
		}

		public Utf8String AccountId
		{
			set
			{
			}
		}

		public void Set(ref CopyExternalUserInfoByAccountIdOptions other)
		{
		}

		public void Set(ref CopyExternalUserInfoByAccountIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
