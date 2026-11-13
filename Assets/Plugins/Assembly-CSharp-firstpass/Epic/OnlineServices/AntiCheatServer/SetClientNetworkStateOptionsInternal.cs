using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct SetClientNetworkStateOptionsInternal : ISettable<SetClientNetworkStateOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ClientHandle;

		private int m_IsNetworkActive;

		public IntPtr ClientHandle
		{
			set
			{
			}
		}

		public bool IsNetworkActive
		{
			set
			{
			}
		}

		public void Set(ref SetClientNetworkStateOptions other)
		{
		}

		public void Set(ref SetClientNetworkStateOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
