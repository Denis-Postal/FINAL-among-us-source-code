namespace Epic.OnlineServices.UI
{
	public struct PS5MemoryMonitorCallbackInfo : ICallbackInfo
	{
		public object ClientData { get; set; }

		public PS5MemoryMonitorCallbackInfoSystemMemoryMonitorReport? SystemMemoryMonitorReport { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref PS5MemoryMonitorCallbackInfoInternal other)
		{
		}
	}
}
