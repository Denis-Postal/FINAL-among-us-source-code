using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct LobbyDetailsGetMemberCountOptionsInternal : ISettable<LobbyDetailsGetMemberCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref LobbyDetailsGetMemberCountOptions other)
		{
		}

		public void Set(ref LobbyDetailsGetMemberCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
