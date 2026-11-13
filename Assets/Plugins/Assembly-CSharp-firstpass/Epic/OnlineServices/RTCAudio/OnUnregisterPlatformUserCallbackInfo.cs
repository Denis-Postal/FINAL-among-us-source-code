namespace Epic.OnlineServices.RTCAudio
{
	public struct OnUnregisterPlatformUserCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Utf8String PlatformUserId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref OnUnregisterPlatformUserCallbackInfoInternal other)
		{
		}
	}
}
