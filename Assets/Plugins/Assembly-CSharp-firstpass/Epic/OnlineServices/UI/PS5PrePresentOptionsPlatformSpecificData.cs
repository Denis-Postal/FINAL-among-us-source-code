using System;

namespace Epic.OnlineServices.UI
{
	public struct PS5PrePresentOptionsPlatformSpecificData : ICallbackInfo
	{
		public IntPtr ScreenRenderTarget { get; set; }

		public PS5SubmitGraphicsCommandsCallback SubmitGraphicsCommands { get; set; }

		public object ClientData { get; set; }

		public Rect? ScreenRenderTargetSubRegion { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref PS5PrePresentOptionsPlatformSpecificDataInternal other)
		{
		}
	}
}
