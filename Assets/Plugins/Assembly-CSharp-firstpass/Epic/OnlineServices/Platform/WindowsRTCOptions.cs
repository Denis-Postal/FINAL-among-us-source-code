namespace Epic.OnlineServices.Platform
{
	public struct WindowsRTCOptions
	{
		public WindowsRTCOptionsPlatformSpecificOptions? PlatformSpecificOptions { get; set; }

		public RTCBackgroundMode BackgroundMode { get; set; }

		internal void Set(ref WindowsRTCOptionsInternal other)
		{
		}
	}
}
