using System;

namespace Epic.OnlineServices.UI
{
	public struct XSXPrePresentOptionsPlatformSpecificData : ICallbackInfo
	{
		public IntPtr ScreenRenderTarget { get; set; }

		public XSXSubmitGraphicsCommandsCallback SubmitGraphicsCommands { get; set; }

		public object ClientData { get; set; }

		public Rect? ScreenRenderTargetSubRegion { get; set; }

		public XSXPrePresentAdditionalData? AdditionalData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref XSXPrePresentOptionsPlatformSpecificDataInternal other)
		{
		}
	}
}
