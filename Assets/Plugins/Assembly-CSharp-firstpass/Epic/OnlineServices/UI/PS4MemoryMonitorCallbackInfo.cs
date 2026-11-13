namespace Epic.OnlineServices.UI
{
	public struct PS4MemoryMonitorCallbackInfo : ICallbackInfo
	{
		public object ClientData { get; set; }

		public PS4MemoryMonitorCallbackInfoSystemMemoryMonitorReport? SystemMemoryMonitorReport { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref PS4MemoryMonitorCallbackInfoInternal other)
		{
		}
	}
}
