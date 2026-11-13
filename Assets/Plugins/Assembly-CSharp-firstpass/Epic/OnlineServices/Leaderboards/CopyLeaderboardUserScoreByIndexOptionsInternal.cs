using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct CopyLeaderboardUserScoreByIndexOptionsInternal : ISettable<CopyLeaderboardUserScoreByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_LeaderboardUserScoreIndex;

		private IntPtr m_StatName;

		public uint LeaderboardUserScoreIndex
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

		public void Set(ref CopyLeaderboardUserScoreByIndexOptions other)
		{
		}

		public void Set(ref CopyLeaderboardUserScoreByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
