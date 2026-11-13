namespace Epic.OnlineServices.TitleStorage
{
	public struct QueryFileListCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public uint FileCount { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref QueryFileListCallbackInfoInternal other)
		{
		}
	}
}
