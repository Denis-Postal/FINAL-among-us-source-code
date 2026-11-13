using System;

namespace Epic.OnlineServices.UI
{
	public struct PS5SubmitGraphicsCommandsCallbackInfo : ICallbackInfo
	{
		public uint BuffersCount { get; set; }

		public IntPtr DispatchCommandBuffers { get; set; }

		public uint? DispatchCommandBuffersSizes { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref PS5SubmitGraphicsCommandsCallbackInfoInternal other)
		{
		}
	}
}
