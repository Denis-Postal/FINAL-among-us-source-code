using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyLobbyInviteReceivedOptionsInternal : ISettable<AddNotifyLobbyInviteReceivedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyLobbyInviteReceivedOptions other)
		{
		}

		public void Set(ref AddNotifyLobbyInviteReceivedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
