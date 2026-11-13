using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyLobbyMemberUpdateReceivedOptionsInternal : ISettable<AddNotifyLobbyMemberUpdateReceivedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyLobbyMemberUpdateReceivedOptions other)
		{
		}

		public void Set(ref AddNotifyLobbyMemberUpdateReceivedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
