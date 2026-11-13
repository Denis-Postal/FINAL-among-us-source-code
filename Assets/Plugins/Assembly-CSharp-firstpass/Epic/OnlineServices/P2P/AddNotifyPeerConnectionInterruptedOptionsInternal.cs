using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct AddNotifyPeerConnectionInterruptedOptionsInternal : ISettable<AddNotifyPeerConnectionInterruptedOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_SocketId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public SocketId? SocketId
		{
			set
			{
			}
		}

		public void Set(ref AddNotifyPeerConnectionInterruptedOptions other)
		{
		}

		public void Set(ref AddNotifyPeerConnectionInterruptedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
