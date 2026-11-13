using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct LogGameRoundEndOptionsInternal : ISettable<LogGameRoundEndOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_WinningTeamId;

		public uint WinningTeamId
		{
			set
			{
			}
		}

		public void Set(ref LogGameRoundEndOptions other)
		{
		}

		public void Set(ref LogGameRoundEndOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
