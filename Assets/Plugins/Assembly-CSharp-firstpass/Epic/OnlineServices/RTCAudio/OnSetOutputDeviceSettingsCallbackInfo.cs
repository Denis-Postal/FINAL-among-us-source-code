namespace Epic.OnlineServices.RTCAudio
{
	public struct OnSetOutputDeviceSettingsCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Utf8String RealDeviceId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref OnSetOutputDeviceSettingsCallbackInfoInternal other)
		{
		}
	}
}
