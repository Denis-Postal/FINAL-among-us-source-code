namespace Epic.OnlineServices.UI
{
	public struct PS5MemoryMonitorCallbackInfoSystemMemoryMonitorReport
	{
		public ulong OverlayUsedCpuMemory { get; set; }

		public ulong OverlayUsedGpuMemory { get; set; }

		internal void Set(ref PS5MemoryMonitorCallbackInfoSystemMemoryMonitorReportInternal other)
		{
		}
	}
}
