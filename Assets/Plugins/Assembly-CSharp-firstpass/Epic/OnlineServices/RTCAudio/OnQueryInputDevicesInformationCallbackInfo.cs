namespace Epic.OnlineServices.RTCAudio
{
	public struct OnQueryInputDevicesInformationCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref OnQueryInputDevicesInformationCallbackInfoInternal other)
		{
		}
	}
}
