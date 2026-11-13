using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct GetLeaderboardDefinitionCountOptionsInternal : ISettable<GetLeaderboardDefinitionCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref GetLeaderboardDefinitionCountOptions other)
		{
		}

		public void Set(ref GetLeaderboardDefinitionCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
