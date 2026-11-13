namespace Epic.OnlineServices.ReceiptValidator
{
	public struct EGSDLCVerifyPurchaseOptionsPlatformSpecificOptions
	{
		public Utf8String CatalogItemId { get; set; }

		public Utf8String EntitlementId { get; set; }

		internal void Set(ref EGSDLCVerifyPurchaseOptionsPlatformSpecificOptionsInternal other)
		{
		}
	}
}
