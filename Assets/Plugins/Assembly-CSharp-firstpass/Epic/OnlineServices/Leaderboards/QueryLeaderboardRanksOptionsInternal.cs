using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct QueryLeaderboardRanksOptionsInternal : ISettable<QueryLeaderboardRanksOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LeaderboardId;

		private IntPtr m_LocalUserId;

		public Utf8String LeaderboardId
		{
			set
			{
			}
		}

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref QueryLeaderboardRanksOptions other)
		{
		}

		public void Set(ref QueryLeaderboardRanksOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
