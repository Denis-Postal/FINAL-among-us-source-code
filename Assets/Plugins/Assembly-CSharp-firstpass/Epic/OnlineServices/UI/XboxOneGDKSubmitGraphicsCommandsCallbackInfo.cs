using System;

namespace Epic.OnlineServices.UI
{
	public struct XboxOneGDKSubmitGraphicsCommandsCallbackInfo : ICallbackInfo
	{
		public XboxOneGDKSubmitGraphicsCommandsDataCommand[] Commands { get; set; }

		public ArraySegment<byte>[] DeviceObjects { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref XboxOneGDKSubmitGraphicsCommandsCallbackInfoInternal other)
		{
		}
	}
}
