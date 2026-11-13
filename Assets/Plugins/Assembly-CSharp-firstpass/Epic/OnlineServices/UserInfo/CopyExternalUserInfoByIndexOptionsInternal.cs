using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct CopyExternalUserInfoByIndexOptionsInternal : ISettable<CopyExternalUserInfoByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

		private uint m_Index;

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

		public uint Index
		{
			set
			{
			}
		}

		public void Set(ref CopyExternalUserInfoByIndexOptions other)
		{
		}

		public void Set(ref CopyExternalUserInfoByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
