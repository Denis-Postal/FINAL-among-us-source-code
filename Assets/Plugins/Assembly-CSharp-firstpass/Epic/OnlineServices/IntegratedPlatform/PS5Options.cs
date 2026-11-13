namespace Epic.OnlineServices.IntegratedPlatform
{
	public struct PS5Options
	{
		public Utf8String Type { get; set; }

		public IntegratedPlatformManagementFlags Flags { get; set; }

		public PS5OptionsInitOptions? InitOptions { get; set; }

		internal void Set(ref PS5OptionsInternal other)
		{
		}
	}
}
