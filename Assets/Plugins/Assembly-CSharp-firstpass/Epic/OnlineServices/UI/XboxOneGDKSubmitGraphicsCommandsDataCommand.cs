namespace Epic.OnlineServices.UI
{
	public struct XboxOneGDKSubmitGraphicsCommandsDataCommand
	{
		public XboxOneGDKSubmitGraphicsCommandsDataAction Action { get; set; }

		public uint DeviceObjectCount { get; set; }

		public ulong ExtraParameter { get; set; }

		internal void Set(ref XboxOneGDKSubmitGraphicsCommandsDataCommandInternal other)
		{
		}
	}
}
