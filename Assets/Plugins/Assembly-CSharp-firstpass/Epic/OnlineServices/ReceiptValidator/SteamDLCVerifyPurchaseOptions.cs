namespace Epic.OnlineServices.ReceiptValidator
{
	public struct SteamDLCVerifyPurchaseOptions
	{
		public ProductUserId LocalUserId { get; set; }

		public ProductUserId TargetUserId { get; set; }

		public ReceiptValidatorVendorType VendorType { get; set; }

		public ReceiptValidatorEntitlementType EntitlementType { get; set; }

		public SteamDLCVerifyPurchaseOptionsPlatformSpecificOptions? PlatformSpecificOptions { get; set; }
	}
}
