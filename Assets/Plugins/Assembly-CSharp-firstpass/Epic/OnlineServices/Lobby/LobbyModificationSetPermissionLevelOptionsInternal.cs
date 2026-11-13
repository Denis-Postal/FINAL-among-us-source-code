using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct LobbyModificationSetPermissionLevelOptionsInternal : ISettable<LobbyModificationSetPermissionLevelOptions>, IDisposable
	{
		private int m_ApiVersion;

		private LobbyPermissionLevel m_PermissionLevel;

		public LobbyPermissionLevel PermissionLevel
		{
			set
			{
			}
		}

		public void Set(ref LobbyModificationSetPermissionLevelOptions other)
		{
		}

		public void Set(ref LobbyModificationSetPermissionLevelOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
