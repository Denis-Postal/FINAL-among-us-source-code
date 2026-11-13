namespace Epic.OnlineServices.UI
{
	public struct HideFriendsCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public EpicAccountId LocalUserId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref HideFriendsCallbackInfoInternal other)
		{
		}
	}
}
