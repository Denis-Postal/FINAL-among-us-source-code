using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct LobbySearchSetLobbyIdOptionsInternal : ISettable<LobbySearchSetLobbyIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LobbyId;

		public Utf8String LobbyId
		{
			set
			{
			}
		}

		public void Set(ref LobbySearchSetLobbyIdOptions other)
		{
		}

		public void Set(ref LobbySearchSetLobbyIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
