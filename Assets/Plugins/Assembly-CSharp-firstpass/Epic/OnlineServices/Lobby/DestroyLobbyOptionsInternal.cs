using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct DestroyLobbyOptionsInternal : ISettable<DestroyLobbyOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_LobbyId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String LobbyId
		{
			set
			{
			}
		}

		public void Set(ref DestroyLobbyOptions other)
		{
		}

		public void Set(ref DestroyLobbyOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
