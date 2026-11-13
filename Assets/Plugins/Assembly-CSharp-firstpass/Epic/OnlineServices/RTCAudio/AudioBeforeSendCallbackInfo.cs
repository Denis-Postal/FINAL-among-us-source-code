namespace Epic.OnlineServices.RTCAudio
{
	public struct AudioBeforeSendCallbackInfo : ICallbackInfo
	{
		public object ClientData { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public Utf8String RoomName { get; set; }

		public AudioBuffer? Buffer { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref AudioBeforeSendCallbackInfoInternal other)
		{
		}
	}
}
