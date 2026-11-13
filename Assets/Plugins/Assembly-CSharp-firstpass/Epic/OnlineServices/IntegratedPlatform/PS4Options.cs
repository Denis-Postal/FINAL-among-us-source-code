namespace Epic.OnlineServices.IntegratedPlatform
{
	public struct PS4Options
	{
		public Utf8String Type { get; set; }

		public IntegratedPlatformManagementFlags Flags { get; set; }

		public PS4OptionsInitOptions? InitOptions { get; set; }

		internal void Set(ref PS4OptionsInternal other)
		{
		}
	}
}
