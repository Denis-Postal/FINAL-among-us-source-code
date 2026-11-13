using System;

namespace Epic.OnlineServices.ReceiptValidator
{
	public struct VerifyPurchaseOptions
	{
		public ProductUserId LocalUserId { get; set; }

		public ProductUserId TargetUserId { get; set; }

		public ReceiptValidatorVendorType VendorType { get; set; }

		public ReceiptValidatorEntitlementType EntitlementType { get; set; }

		public IntPtr PlatformSpecificOptions { get; set; }
	}
}
