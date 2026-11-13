namespace Epic.OnlineServices.ReceiptValidator
{
	public struct PS5IAPVerifyPurchaseOptions
	{
		public ProductUserId LocalUserId { get; set; }

		public ProductUserId TargetUserId { get; set; }

		public ReceiptValidatorVendorType VendorType { get; set; }

		public ReceiptValidatorEntitlementType EntitlementType { get; set; }

		public PS5IAPVerifyPurchaseOptionsPlatformSpecificOptions? PlatformSpecificOptions { get; set; }
	}
}
