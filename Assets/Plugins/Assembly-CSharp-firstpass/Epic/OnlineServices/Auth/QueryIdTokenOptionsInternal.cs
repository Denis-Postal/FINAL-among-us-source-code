using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct QueryIdTokenOptionsInternal : ISettable<QueryIdTokenOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetAccountId;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public EpicAccountId TargetAccountId
		{
			set
			{
			}
		}

		public void Set(ref QueryIdTokenOptions other)
		{
		}

		public void Set(ref QueryIdTokenOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
