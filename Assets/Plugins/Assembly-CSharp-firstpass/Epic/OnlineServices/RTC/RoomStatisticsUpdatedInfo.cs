namespace Epic.OnlineServices.RTC
{
	public struct RoomStatisticsUpdatedInfo : ICallbackInfo
	{
		public object ClientData { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public Utf8String RoomName { get; set; }

		public Utf8String Statistic { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref RoomStatisticsUpdatedInfoInternal other)
		{
		}
	}
}
