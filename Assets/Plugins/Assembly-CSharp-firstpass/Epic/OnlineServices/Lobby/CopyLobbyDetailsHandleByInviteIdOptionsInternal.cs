using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct CopyLobbyDetailsHandleByInviteIdOptionsInternal : ISettable<CopyLobbyDetailsHandleByInviteIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_InviteId;

		public Utf8String InviteId
		{
			set
			{
			}
		}

		public void Set(ref CopyLobbyDetailsHandleByInviteIdOptions other)
		{
		}

		public void Set(ref CopyLobbyDetailsHandleByInviteIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
