namespace Epic.OnlineServices.Sessions
{
	public struct SessionSearchFindCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref SessionSearchFindCallbackInfoInternal other)
		{
		}
	}
}
