using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct LobbyModificationRemoveMemberAttributeOptionsInternal : ISettable<LobbyModificationRemoveMemberAttributeOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Key;

		public Utf8String Key
		{
			set
			{
			}
		}

		public void Set(ref LobbyModificationRemoveMemberAttributeOptions other)
		{
		}

		public void Set(ref LobbyModificationRemoveMemberAttributeOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
