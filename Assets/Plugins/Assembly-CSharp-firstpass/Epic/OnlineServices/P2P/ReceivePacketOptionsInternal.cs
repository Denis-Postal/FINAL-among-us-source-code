using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct ReceivePacketOptionsInternal : IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private uint m_MaxDataSizeBytes;

		public IntPtr m_RequestedChannel;

		public ReceivePacketOptionsInternal(ref ReceivePacketOptions other)
		{
			m_ApiVersion = 0;
			m_LocalUserId = (IntPtr)0;
			m_MaxDataSizeBytes = 0u;
			m_RequestedChannel = (IntPtr)0;
		}

		public void Dispose()
		{
		}
	}
}
