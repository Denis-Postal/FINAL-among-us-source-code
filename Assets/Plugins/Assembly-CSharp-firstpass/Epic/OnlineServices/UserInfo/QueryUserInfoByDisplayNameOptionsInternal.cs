using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct QueryUserInfoByDisplayNameOptionsInternal : ISettable<QueryUserInfoByDisplayNameOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_DisplayName;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String DisplayName
		{
			set
			{
			}
		}

		public void Set(ref QueryUserInfoByDisplayNameOptions other)
		{
		}

		public void Set(ref QueryUserInfoByDisplayNameOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
