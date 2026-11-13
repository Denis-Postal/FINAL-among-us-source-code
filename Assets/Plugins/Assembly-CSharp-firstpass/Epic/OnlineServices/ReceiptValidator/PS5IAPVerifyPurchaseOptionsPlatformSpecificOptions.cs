namespace Epic.OnlineServices.ReceiptValidator
{
	public struct PS5IAPVerifyPurchaseOptionsPlatformSpecificOptions
	{
		public Utf8String EntitlementLabel { get; set; }

		public Utf8String ServiceLabel { get; set; }

		public Utf8String IdToken { get; set; }

		public Utf8String AccessToken { get; set; }

		internal void Set(ref PS5IAPVerifyPurchaseOptionsPlatformSpecificOptionsInternal other)
		{
		}
	}
}
