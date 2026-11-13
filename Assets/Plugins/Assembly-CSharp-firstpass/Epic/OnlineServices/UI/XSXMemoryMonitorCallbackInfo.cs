namespace Epic.OnlineServices.UI
{
	public struct XSXMemoryMonitorCallbackInfo : ICallbackInfo
	{
		public object ClientData { get; set; }

		public XSXMemoryMonitorCallbackInfoSystemMemoryMonitorReport? SystemMemoryMonitorReport { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref XSXMemoryMonitorCallbackInfoInternal other)
		{
		}
	}
}
