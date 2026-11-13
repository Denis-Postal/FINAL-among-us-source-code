using System;

namespace Epic.OnlineServices.UI
{
	public struct XSXSubmitGraphicsCommandsCallbackInfo : ICallbackInfo
	{
		public XSXSubmitGraphicsCommandsDataCommand[] Commands { get; set; }

		public ArraySegment<byte>[] DeviceObjects { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref XSXSubmitGraphicsCommandsCallbackInfoInternal other)
		{
		}
	}
}
