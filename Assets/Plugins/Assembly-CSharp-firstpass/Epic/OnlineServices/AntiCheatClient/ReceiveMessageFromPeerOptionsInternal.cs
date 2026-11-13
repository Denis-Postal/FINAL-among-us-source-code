using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct ReceiveMessageFromPeerOptionsInternal : ISettable<ReceiveMessageFromPeerOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PeerHandle;

		private uint m_DataLengthBytes;

		private IntPtr m_Data;

		public IntPtr PeerHandle
		{
			set
			{
			}
		}

		public ArraySegment<byte> Data
		{
			set
			{
			}
		}

		public void Set(ref ReceiveMessageFromPeerOptions other)
		{
		}

		public void Set(ref ReceiveMessageFromPeerOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
