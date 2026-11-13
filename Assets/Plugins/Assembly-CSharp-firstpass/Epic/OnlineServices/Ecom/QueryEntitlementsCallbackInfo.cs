namespace Epic.OnlineServices.Ecom
{
	public struct QueryEntitlementsCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public EpicAccountId LocalUserId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref QueryEntitlementsCallbackInfoInternal other)
		{
		}
	}
}
