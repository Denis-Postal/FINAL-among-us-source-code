namespace Epic.OnlineServices.ReceiptValidator
{
	public struct XSXIAPVerifyPurchaseOptions
	{
		public ProductUserId LocalUserId { get; set; }

		public ProductUserId TargetUserId { get; set; }

		public ReceiptValidatorVendorType VendorType { get; set; }

		public ReceiptValidatorEntitlementType EntitlementType { get; set; }

		public XSXIAPVerifyPurchaseOptionsPlatformSpecificOptions? PlatformSpecificOptions { get; set; }
	}
}
