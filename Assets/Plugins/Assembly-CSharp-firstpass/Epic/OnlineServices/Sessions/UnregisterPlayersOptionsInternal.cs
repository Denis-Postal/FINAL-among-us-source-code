using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct UnregisterPlayersOptionsInternal : ISettable<UnregisterPlayersOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_SessionName;

		private IntPtr m_PlayersToUnregister;

		private uint m_PlayersToUnregisterCount;

		public Utf8String SessionName
		{
			set
			{
			}
		}

		public ProductUserId[] PlayersToUnregister
		{
			set
			{
			}
		}

		public void Set(ref UnregisterPlayersOptions other)
		{
		}

		public void Set(ref UnregisterPlayersOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
