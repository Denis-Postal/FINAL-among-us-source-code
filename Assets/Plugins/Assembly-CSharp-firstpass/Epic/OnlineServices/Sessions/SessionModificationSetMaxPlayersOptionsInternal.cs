using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct SessionModificationSetMaxPlayersOptionsInternal : ISettable<SessionModificationSetMaxPlayersOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_MaxPlayers;

		public uint MaxPlayers
		{
			set
			{
			}
		}

		public void Set(ref SessionModificationSetMaxPlayersOptions other)
		{
		}

		public void Set(ref SessionModificationSetMaxPlayersOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
