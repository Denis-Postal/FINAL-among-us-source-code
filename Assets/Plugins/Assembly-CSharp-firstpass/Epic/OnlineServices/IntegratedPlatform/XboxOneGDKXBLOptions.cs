namespace Epic.OnlineServices.IntegratedPlatform
{
	public struct XboxOneGDKXBLOptions
	{
		public Utf8String Type { get; set; }

		public IntegratedPlatformManagementFlags Flags { get; set; }

		public XboxOneGDKXBLOptionsInitOptions? InitOptions { get; set; }

		internal void Set(ref XboxOneGDKXBLOptionsInternal other)
		{
		}
	}
}
