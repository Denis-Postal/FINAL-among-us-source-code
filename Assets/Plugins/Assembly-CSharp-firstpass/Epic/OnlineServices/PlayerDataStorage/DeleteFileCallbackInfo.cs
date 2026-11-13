namespace Epic.OnlineServices.PlayerDataStorage
{
	public struct DeleteFileCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref DeleteFileCallbackInfoInternal other)
		{
		}
	}
}
