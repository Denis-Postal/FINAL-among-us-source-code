using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct UpdateLobbyOptionsInternal : ISettable<UpdateLobbyOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LobbyModificationHandle;

		public LobbyModification LobbyModificationHandle
		{
			set
			{
			}
		}

		public void Set(ref UpdateLobbyOptions other)
		{
		}

		public void Set(ref UpdateLobbyOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
