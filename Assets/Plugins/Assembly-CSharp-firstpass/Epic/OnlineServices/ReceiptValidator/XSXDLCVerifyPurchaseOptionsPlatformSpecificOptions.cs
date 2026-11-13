namespace Epic.OnlineServices.ReceiptValidator
{
	public struct XSXDLCVerifyPurchaseOptionsPlatformSpecificOptions
	{
		public Utf8String ItemId { get; set; }

		public Utf8String XSTSToken { get; set; }

		internal void Set(ref XSXDLCVerifyPurchaseOptionsPlatformSpecificOptionsInternal other)
		{
		}
	}
}
