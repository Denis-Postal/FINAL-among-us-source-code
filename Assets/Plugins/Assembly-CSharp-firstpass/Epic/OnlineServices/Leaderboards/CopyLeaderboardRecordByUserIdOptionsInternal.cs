using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct CopyLeaderboardRecordByUserIdOptionsInternal : ISettable<CopyLeaderboardRecordByUserIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_UserId;

		public ProductUserId UserId
		{
			set
			{
			}
		}

		public void Set(ref CopyLeaderboardRecordByUserIdOptions other)
		{
		}

		public void Set(ref CopyLeaderboardRecordByUserIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
