using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct GetLeaderboardUserScoreCountOptionsInternal : ISettable<GetLeaderboardUserScoreCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_StatName;

		public Utf8String StatName
		{
			set
			{
			}
		}

		public void Set(ref GetLeaderboardUserScoreCountOptions other)
		{
		}

		public void Set(ref GetLeaderboardUserScoreCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
