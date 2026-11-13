using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct UnregisterPeerOptionsInternal : ISettable<UnregisterPeerOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PeerHandle;

		public IntPtr PeerHandle
		{
			set
			{
			}
		}

		public void Set(ref UnregisterPeerOptions other)
		{
		}

		public void Set(ref UnregisterPeerOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
