namespace Epic.OnlineServices.Platform
{
	public enum DesktopCrossplayStatus
	{
		Ok = 0,
		ApplicationNotBootstrapped = 1,
		ServiceNotInstalled = 2,
		ServiceStartFailed = 3,
		ServiceNotRunning = 4,
		OverlayDisabled = 5,
		OverlayNotInstalled = 6,
		OverlayTrustCheckFailed = 7,
		OverlayLoadFailed = 8
	}
}
