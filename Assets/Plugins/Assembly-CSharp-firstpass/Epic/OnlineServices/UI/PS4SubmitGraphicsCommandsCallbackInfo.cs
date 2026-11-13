using System;

namespace Epic.OnlineServices.UI
{
	public struct PS4SubmitGraphicsCommandsCallbackInfo : ICallbackInfo
	{
		public uint BuffersCount { get; set; }

		public IntPtr DispatchCommandBuffers { get; set; }

		public uint? DispatchCommandBuffersSizes { get; set; }

		public object ClientData { get; set; }

		public IntPtr DispatchCommandBuffersReturnPatches { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref PS4SubmitGraphicsCommandsCallbackInfoInternal other)
		{
		}
	}
}
