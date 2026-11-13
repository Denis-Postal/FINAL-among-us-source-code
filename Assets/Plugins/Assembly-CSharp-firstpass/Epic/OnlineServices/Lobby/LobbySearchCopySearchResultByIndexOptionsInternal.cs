using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct LobbySearchCopySearchResultByIndexOptionsInternal : ISettable<LobbySearchCopySearchResultByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_LobbyIndex;

		public uint LobbyIndex
		{
			set
			{
			}
		}

		public void Set(ref LobbySearchCopySearchResultByIndexOptions other)
		{
		}

		public void Set(ref LobbySearchCopySearchResultByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
