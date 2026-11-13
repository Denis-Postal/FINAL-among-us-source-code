using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct LobbyDetailsCopyAttributeByIndexOptionsInternal : ISettable<LobbyDetailsCopyAttributeByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_AttrIndex;

		public uint AttrIndex
		{
			set
			{
			}
		}

		public void Set(ref LobbyDetailsCopyAttributeByIndexOptions other)
		{
		}

		public void Set(ref LobbyDetailsCopyAttributeByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
