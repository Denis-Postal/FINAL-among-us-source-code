using System;

namespace Epic.OnlineServices.UI
{
	public struct PS4PrePresentOptionsPlatformSpecificData : ICallbackInfo
	{
		public IntPtr ScreenRenderTarget { get; set; }

		public PS4SubmitGraphicsCommandsCallback SubmitGraphicsCommands { get; set; }

		public object ClientData { get; set; }

		public Rect? ScreenRenderTargetSubRegion { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref PS4PrePresentOptionsPlatformSpecificDataInternal other)
		{
		}
	}
}
