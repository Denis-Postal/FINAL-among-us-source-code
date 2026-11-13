namespace Epic.OnlineServices.ReceiptValidator
{
	public struct EGSIAPVerifyPurchaseOptionsPlatformSpecificOptions
	{
		public Utf8String EntitlementName { get; set; }

		public Utf8String EntitlementId { get; set; }

		internal void Set(ref EGSIAPVerifyPurchaseOptionsPlatformSpecificOptionsInternal other)
		{
		}
	}
}
