namespace Epic.OnlineServices.Auth
{
	public struct DeletePersistentAuthCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref DeletePersistentAuthCallbackInfoInternal other)
		{
		}
	}
}
