namespace Epic.OnlineServices.Friends
{
	public struct QueryFriendsCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public EpicAccountId LocalUserId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref QueryFriendsCallbackInfoInternal other)
		{
		}
	}
}
