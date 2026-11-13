using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct GetLeaderboardRecordCountOptionsInternal : ISettable<GetLeaderboardRecordCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref GetLeaderboardRecordCountOptions other)
		{
		}

		public void Set(ref GetLeaderboardRecordCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
