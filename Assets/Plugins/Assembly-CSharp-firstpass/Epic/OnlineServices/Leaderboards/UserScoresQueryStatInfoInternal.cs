using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct UserScoresQueryStatInfoInternal : IGettable<UserScoresQueryStatInfo>, ISettable<UserScoresQueryStatInfo>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_StatName;

		private LeaderboardAggregation m_Aggregation;

		public Utf8String StatName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public LeaderboardAggregation Aggregation
		{
			get
			{
				return LeaderboardAggregation.Min;
			}
			set
			{
			}
		}

		public void Set(ref UserScoresQueryStatInfo other)
		{
		}

		public void Set(ref UserScoresQueryStatInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out UserScoresQueryStatInfo output)
		{
			output = default;
		}
	}
}
