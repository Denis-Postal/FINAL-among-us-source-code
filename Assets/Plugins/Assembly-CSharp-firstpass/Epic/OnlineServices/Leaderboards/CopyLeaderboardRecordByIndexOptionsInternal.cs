using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct CopyLeaderboardRecordByIndexOptionsInternal : ISettable<CopyLeaderboardRecordByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_LeaderboardRecordIndex;

		public uint LeaderboardRecordIndex
		{
			set
			{
			}
		}

		public void Set(ref CopyLeaderboardRecordByIndexOptions other)
		{
		}

		public void Set(ref CopyLeaderboardRecordByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
