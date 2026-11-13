namespace Epic.OnlineServices.RTCAdmin
{
	public struct KickCompleteCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref KickCompleteCallbackInfoInternal other)
		{
		}
	}
}
