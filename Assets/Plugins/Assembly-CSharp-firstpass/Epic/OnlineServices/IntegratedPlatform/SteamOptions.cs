namespace Epic.OnlineServices.IntegratedPlatform
{
	public struct SteamOptions
	{
		public Utf8String OverrideLibraryPath { get; set; }

		public uint SteamMajorVersion { get; set; }

		public uint SteamMinorVersion { get; set; }

		public Utf8String SteamApiInterfaceVersionsArray { get; set; }

		public uint SteamApiInterfaceVersionsArrayBytes { get; set; }

		internal void Set(ref SteamOptionsInternal other)
		{
		}
	}
}
