using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LeaderboardUserScoreInternal : IGettable<LeaderboardUserScore>, ISettable<LeaderboardUserScore>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_UserId;

		private int m_Score;

		public ProductUserId UserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public int Score
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public void Set(ref LeaderboardUserScore other)
		{
		}

		public void Set(ref LeaderboardUserScore? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LeaderboardUserScore output)
		{
			output = default;
		}
	}
}
