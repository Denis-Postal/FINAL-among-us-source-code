namespace Epic.OnlineServices.Sessions
{
	public struct DestroySessionCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref DestroySessionCallbackInfoInternal other)
		{
		}
	}
}
