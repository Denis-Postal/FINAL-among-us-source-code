using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct LobbySearchFindOptionsInternal : ISettable<LobbySearchFindOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref LobbySearchFindOptions other)
		{
		}

		public void Set(ref LobbySearchFindOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
