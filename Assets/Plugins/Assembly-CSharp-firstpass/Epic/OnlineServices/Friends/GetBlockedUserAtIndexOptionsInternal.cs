using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct GetBlockedUserAtIndexOptionsInternal : ISettable<GetBlockedUserAtIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private int m_Index;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public int Index
		{
			set
			{
			}
		}

		public void Set(ref GetBlockedUserAtIndexOptions other)
		{
		}

		public void Set(ref GetBlockedUserAtIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
