using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct CopyLeaderboardDefinitionByLeaderboardIdOptionsInternal : ISettable<CopyLeaderboardDefinitionByLeaderboardIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LeaderboardId;

		public Utf8String LeaderboardId
		{
			set
			{
			}
		}

		public void Set(ref CopyLeaderboardDefinitionByLeaderboardIdOptions other)
		{
		}

		public void Set(ref CopyLeaderboardDefinitionByLeaderboardIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
