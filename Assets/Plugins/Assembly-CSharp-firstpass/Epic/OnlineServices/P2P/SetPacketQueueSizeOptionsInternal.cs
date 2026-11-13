using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct SetPacketQueueSizeOptionsInternal : ISettable<SetPacketQueueSizeOptions>, IDisposable
	{
		private int m_ApiVersion;

		private ulong m_IncomingPacketQueueMaxSizeBytes;

		private ulong m_OutgoingPacketQueueMaxSizeBytes;

		public ulong IncomingPacketQueueMaxSizeBytes
		{
			set
			{
			}
		}

		public ulong OutgoingPacketQueueMaxSizeBytes
		{
			set
			{
			}
		}

		public void Set(ref SetPacketQueueSizeOptions other)
		{
		}

		public void Set(ref SetPacketQueueSizeOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
