using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyLeaderboardUserScoreByUserIdOptionsInternal : ISettable<CopyLeaderboardUserScoreByUserIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_UserId;

		private IntPtr m_StatName;

		public ProductUserId UserId
		{
			set
			{
			}
		}

		public Utf8String StatName
		{
			set
			{
			}
		}

		public void Set(ref CopyLeaderboardUserScoreByUserIdOptions other)
		{
		}

		public void Set(ref CopyLeaderboardUserScoreByUserIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
