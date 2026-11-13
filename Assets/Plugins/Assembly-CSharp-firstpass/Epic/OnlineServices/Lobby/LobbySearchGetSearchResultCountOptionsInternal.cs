using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct LobbySearchGetSearchResultCountOptionsInternal : ISettable<LobbySearchGetSearchResultCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref LobbySearchGetSearchResultCountOptions other)
		{
		}

		public void Set(ref LobbySearchGetSearchResultCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
