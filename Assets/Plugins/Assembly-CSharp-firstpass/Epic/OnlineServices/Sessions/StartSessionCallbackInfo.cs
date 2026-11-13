namespace Epic.OnlineServices.Sessions
{
	public struct StartSessionCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref StartSessionCallbackInfoInternal other)
		{
		}
	}
}
