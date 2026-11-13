using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct LobbySearchSetMaxResultsOptionsInternal : ISettable<LobbySearchSetMaxResultsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_MaxResults;

		public uint MaxResults
		{
			set
			{
			}
		}

		public void Set(ref LobbySearchSetMaxResultsOptions other)
		{
		}

		public void Set(ref LobbySearchSetMaxResultsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
