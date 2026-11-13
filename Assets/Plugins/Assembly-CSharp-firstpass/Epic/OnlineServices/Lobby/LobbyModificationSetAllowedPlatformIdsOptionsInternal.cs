using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LobbyModificationSetAllowedPlatformIdsOptionsInternal : ISettable<LobbyModificationSetAllowedPlatformIdsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_AllowedPlatformIds;

		private uint m_AllowedPlatformIdsCount;

		public uint[] AllowedPlatformIds
		{
			set
			{
			}
		}

		public void Set(ref LobbyModificationSetAllowedPlatformIdsOptions other)
		{
		}

		public void Set(ref LobbyModificationSetAllowedPlatformIdsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
