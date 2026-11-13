using System;

namespace Epic.OnlineServices.UI
{
	public struct XboxOneGDKPrePresentOptionsPlatformSpecificData : ICallbackInfo
	{
		public IntPtr ScreenRenderTarget { get; set; }

		public XboxOneGDKSubmitGraphicsCommandsCallback SubmitGraphicsCommands { get; set; }

		public object ClientData { get; set; }

		public Rect? ScreenRenderTargetSubRegion { get; set; }

		public XboxOneGDKPrePresentAdditionalData? AdditionalData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref XboxOneGDKPrePresentOptionsPlatformSpecificDataInternal other)
		{
		}
	}
}
