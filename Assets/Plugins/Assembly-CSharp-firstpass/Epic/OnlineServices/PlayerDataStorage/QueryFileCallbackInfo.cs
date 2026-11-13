namespace Epic.OnlineServices.PlayerDataStorage
{
	public struct QueryFileCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref QueryFileCallbackInfoInternal other)
		{
		}
	}
}
