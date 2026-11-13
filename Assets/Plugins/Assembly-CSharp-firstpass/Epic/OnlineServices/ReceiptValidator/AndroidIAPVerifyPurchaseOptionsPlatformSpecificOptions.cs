namespace Epic.OnlineServices.ReceiptValidator
{
	public struct AndroidIAPVerifyPurchaseOptionsPlatformSpecificOptions
	{
		public Utf8String ProductId { get; set; }

		public Utf8String Token { get; set; }

		internal void Set(ref AndroidIAPVerifyPurchaseOptionsPlatformSpecificOptionsInternal other)
		{
		}
	}
}
