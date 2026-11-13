namespace Epic.OnlineServices.ReceiptValidator
{
	public struct SteamDLCVerifyPurchaseOptionsPlatformSpecificOptions
	{
		public Utf8String AppId { get; set; }

		public Utf8String SteamId { get; set; }

		internal void Set(ref SteamDLCVerifyPurchaseOptionsPlatformSpecificOptionsInternal other)
		{
		}
	}
}
