namespace Epic.OnlineServices.IntegratedPlatform
{
	public struct XSXXBLOptions
	{
		public Utf8String Type { get; set; }

		public IntegratedPlatformManagementFlags Flags { get; set; }

		public XSXXBLOptionsInitOptions? InitOptions { get; set; }

		internal void Set(ref XSXXBLOptionsInternal other)
		{
		}
	}
}
