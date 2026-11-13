namespace Epic.OnlineServices.KWS
{
	public struct QueryAgeGateCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Utf8String CountryCode { get; set; }

		public uint AgeOfConsent { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref QueryAgeGateCallbackInfoInternal other)
		{
		}
	}
}
