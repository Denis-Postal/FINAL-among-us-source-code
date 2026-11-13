using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct LobbyDetailsCopyAttributeByKeyOptionsInternal : ISettable<LobbyDetailsCopyAttributeByKeyOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_AttrKey;

		public Utf8String AttrKey
		{
			set
			{
			}
		}

		public void Set(ref LobbyDetailsCopyAttributeByKeyOptions other)
		{
		}

		public void Set(ref LobbyDetailsCopyAttributeByKeyOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
