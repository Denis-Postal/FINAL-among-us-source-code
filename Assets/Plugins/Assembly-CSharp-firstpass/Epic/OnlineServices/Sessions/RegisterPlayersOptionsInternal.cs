using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct RegisterPlayersOptionsInternal : ISettable<RegisterPlayersOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_SessionName;

		private IntPtr m_PlayersToRegister;

		private uint m_PlayersToRegisterCount;

		public Utf8String SessionName
		{
			set
			{
			}
		}

		public ProductUserId[] PlayersToRegister
		{
			set
			{
			}
		}

		public void Set(ref RegisterPlayersOptions other)
		{
		}

		public void Set(ref RegisterPlayersOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
