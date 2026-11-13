using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyIncomingPacketQueueFullOptionsInternal : ISettable<AddNotifyIncomingPacketQueueFullOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyIncomingPacketQueueFullOptions other)
		{
		}

		public void Set(ref AddNotifyIncomingPacketQueueFullOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
