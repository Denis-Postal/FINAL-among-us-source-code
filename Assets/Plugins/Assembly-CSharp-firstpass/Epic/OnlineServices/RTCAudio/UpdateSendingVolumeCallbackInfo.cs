namespace Epic.OnlineServices.RTCAudio
{
	public struct UpdateSendingVolumeCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public Utf8String RoomName { get; set; }

		public float Volume { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref UpdateSendingVolumeCallbackInfoInternal other)
		{
		}
	}
}
