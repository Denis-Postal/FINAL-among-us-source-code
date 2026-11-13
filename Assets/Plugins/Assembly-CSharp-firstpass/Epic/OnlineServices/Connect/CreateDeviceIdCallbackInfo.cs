namespace Epic.OnlineServices.Connect
{
	public struct CreateDeviceIdCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref CreateDeviceIdCallbackInfoInternal other)
		{
		}
	}
}
