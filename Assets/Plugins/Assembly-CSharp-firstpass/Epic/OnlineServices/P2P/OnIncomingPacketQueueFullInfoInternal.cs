using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct OnIncomingPacketQueueFullInfoInternal : ICallbackInfoInternal, IGettable<OnIncomingPacketQueueFullInfo>, ISettable<OnIncomingPacketQueueFullInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private ulong m_PacketQueueMaxSizeBytes;

		private ulong m_PacketQueueCurrentSizeBytes;

		private IntPtr m_OverflowPacketLocalUserId;

		private byte m_OverflowPacketChannel;

		private uint m_OverflowPacketSizeBytes;

		public object ClientData
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public IntPtr ClientDataAddress => (IntPtr)0;

		public ulong PacketQueueMaxSizeBytes
		{
			get
			{
				return 0uL;
			}
			set
			{
			}
		}

		public ulong PacketQueueCurrentSizeBytes
		{
			get
			{
				return 0uL;
			}
			set
			{
			}
		}

		public ProductUserId OverflowPacketLocalUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public byte OverflowPacketChannel
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public uint OverflowPacketSizeBytes
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public void Set(ref OnIncomingPacketQueueFullInfo other)
		{
		}

		public void Set(ref OnIncomingPacketQueueFullInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnIncomingPacketQueueFullInfo output)
		{
			output = default;
		}
	}
}
