using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 64)]
	internal struct SendPacketOptionsInternal : ISettable<SendPacketOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RemoteUserId;

		internal IntPtr m_SocketId;

		private byte m_Channel;

		private uint m_DataLengthBytes;

		private IntPtr m_Data;

		private int m_AllowDelayedDelivery;

		private PacketReliability m_Reliability;

		private int m_DisableAutoAcceptConnection;

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

		public byte Channel
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

		public bool AllowDelayedDelivery
		{
			set
			{
			}
		}

		public PacketReliability Reliability
		{
			set
			{
			}
		}

		public bool DisableAutoAcceptConnection
		{
			set
			{
			}
		}

		public void Set(ref SendPacketOptions other)
		{
		}

		public void Set(ref SendPacketOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
