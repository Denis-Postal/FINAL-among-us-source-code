namespace Epic.OnlineServices.ReceiptValidator
{
	public struct PS4DLCVerifyPurchaseOptionsPlatformSpecificOptions
	{
		public Utf8String EntitlementLabel { get; set; }

		public Utf8String ServiceLabel { get; set; }

		public Utf8String IdToken { get; set; }

		public Utf8String AccessToken { get; set; }

		internal void Set(ref PS4DLCVerifyPurchaseOptionsPlatformSpecificOptionsInternal other)
		{
		}
	}
}
