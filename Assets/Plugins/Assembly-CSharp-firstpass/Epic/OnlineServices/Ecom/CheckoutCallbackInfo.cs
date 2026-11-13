namespace Epic.OnlineServices.Ecom
{
	public struct CheckoutCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public EpicAccountId LocalUserId { get; set; }

		public Utf8String TransactionId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref CheckoutCallbackInfoInternal other)
		{
		}
	}
}
