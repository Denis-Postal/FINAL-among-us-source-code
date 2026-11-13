using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyLobbyUpdateReceivedOptionsInternal : ISettable<AddNotifyLobbyUpdateReceivedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyLobbyUpdateReceivedOptions other)
		{
		}

		public void Set(ref AddNotifyLobbyUpdateReceivedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
