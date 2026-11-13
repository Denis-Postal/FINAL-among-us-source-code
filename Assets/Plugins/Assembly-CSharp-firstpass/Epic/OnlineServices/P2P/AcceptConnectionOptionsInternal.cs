using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct AcceptConnectionOptionsInternal : ISettable<AcceptConnectionOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RemoteUserId;

		private IntPtr m_SocketId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public ProductUserId RemoteUserId
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

		public void Set(ref AcceptConnectionOptions other)
		{
		}

		public void Set(ref AcceptConnectionOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
