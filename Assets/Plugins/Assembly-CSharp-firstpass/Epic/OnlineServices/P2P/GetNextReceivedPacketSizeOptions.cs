namespace Epic.OnlineServices.P2P
{
	public struct GetNextReceivedPacketSizeOptions
	{
		internal byte[] m_RequestedChannel;

		public ProductUserId LocalUserId { get; set; }

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
