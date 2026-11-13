using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8)]
	public struct ReceivePacketOptions
	{
		internal byte[] m_RequestedChannel;

		public ProductUserId LocalUserId { get; set; }

		public uint MaxDataSizeBytes { get; set; }

		public byte? RequestedChannel
		{
			get
			{
				return null;
			}
			set
			{
			}
		}
	}
}
