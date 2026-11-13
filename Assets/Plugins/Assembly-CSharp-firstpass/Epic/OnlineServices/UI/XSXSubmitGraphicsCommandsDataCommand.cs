namespace Epic.OnlineServices.UI
{
	public struct XSXSubmitGraphicsCommandsDataCommand
	{
		public XSXSubmitGraphicsCommandsDataAction Action { get; set; }

		public uint DeviceObjectCount { get; set; }

		public ulong ExtraParameter { get; set; }

		internal void Set(ref XSXSubmitGraphicsCommandsDataCommandInternal other)
		{
		}
	}
}
