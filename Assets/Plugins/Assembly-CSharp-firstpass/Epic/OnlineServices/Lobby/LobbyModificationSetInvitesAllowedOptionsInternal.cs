using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct LobbyModificationSetInvitesAllowedOptionsInternal : ISettable<LobbyModificationSetInvitesAllowedOptions>, IDisposable
	{
		private int m_ApiVersion;

		private int m_InvitesAllowed;

		public bool InvitesAllowed
		{
			set
			{
			}
		}

		public void Set(ref LobbyModificationSetInvitesAllowedOptions other)
		{
		}

		public void Set(ref LobbyModificationSetInvitesAllowedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
