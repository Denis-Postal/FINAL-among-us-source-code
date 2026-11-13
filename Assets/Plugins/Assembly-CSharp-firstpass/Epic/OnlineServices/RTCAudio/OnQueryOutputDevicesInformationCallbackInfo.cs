namespace Epic.OnlineServices.RTCAudio
{
	public struct OnQueryOutputDevicesInformationCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref OnQueryOutputDevicesInformationCallbackInfoInternal other)
		{
		}
	}
}
