using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct GetNextReceivedPacketSizeOptionsInternal : ISettable<GetNextReceivedPacketSizeOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RequestedChannel;

		public void Set(ref GetNextReceivedPacketSizeOptions other)
		{
		}

		public void Set(ref GetNextReceivedPacketSizeOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
