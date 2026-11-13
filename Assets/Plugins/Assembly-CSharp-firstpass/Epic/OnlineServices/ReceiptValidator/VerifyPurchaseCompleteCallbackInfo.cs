namespace Epic.OnlineServices.ReceiptValidator
{
	public struct VerifyPurchaseCompleteCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref VerifyPurchaseCompleteCallbackInfoInternal other)
		{
		}
	}
}
