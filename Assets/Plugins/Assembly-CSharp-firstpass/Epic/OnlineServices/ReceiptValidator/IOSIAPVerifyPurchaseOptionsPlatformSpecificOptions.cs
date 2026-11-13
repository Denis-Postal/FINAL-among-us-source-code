namespace Epic.OnlineServices.ReceiptValidator
{
	public struct IOSIAPVerifyPurchaseOptionsPlatformSpecificOptions
	{
		public Utf8String TransactionId { get; set; }

		public Utf8String Receipt { get; set; }

		internal void Set(ref IOSIAPVerifyPurchaseOptionsPlatformSpecificOptionsInternal other)
		{
		}
	}
}
