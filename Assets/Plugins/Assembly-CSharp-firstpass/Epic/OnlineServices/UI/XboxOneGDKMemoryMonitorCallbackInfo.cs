namespace Epic.OnlineServices.UI
{
	public struct XboxOneGDKMemoryMonitorCallbackInfo : ICallbackInfo
	{
		public object ClientData { get; set; }

		public XboxOneGDKMemoryMonitorCallbackInfoSystemMemoryMonitorReport? SystemMemoryMonitorReport { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref XboxOneGDKMemoryMonitorCallbackInfoInternal other)
		{
		}
	}
}
