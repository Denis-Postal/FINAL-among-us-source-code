using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyLobbyInviteRejectedOptionsInternal : ISettable<AddNotifyLobbyInviteRejectedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyLobbyInviteRejectedOptions other)
		{
		}

		public void Set(ref AddNotifyLobbyInviteRejectedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
