namespace Epic.OnlineServices.Connect
{
	public struct LoginCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public ContinuanceToken ContinuanceToken { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref LoginCallbackInfoInternal other)
		{
		}
	}
}
