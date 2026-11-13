using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct QueryUserInfoOptionsInternal : ISettable<QueryUserInfoOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

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

		public void Set(ref QueryUserInfoOptions other)
		{
		}

		public void Set(ref QueryUserInfoOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
