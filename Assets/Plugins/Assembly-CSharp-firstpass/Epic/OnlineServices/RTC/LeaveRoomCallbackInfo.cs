namespace Epic.OnlineServices.RTC
{
	public struct LeaveRoomCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public Utf8String RoomName { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref LeaveRoomCallbackInfoInternal other)
		{
		}
	}
}
