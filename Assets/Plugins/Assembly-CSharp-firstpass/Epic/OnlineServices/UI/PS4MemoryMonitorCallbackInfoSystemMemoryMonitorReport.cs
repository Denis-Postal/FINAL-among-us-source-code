namespace Epic.OnlineServices.UI
{
	public struct PS4MemoryMonitorCallbackInfoSystemMemoryMonitorReport
	{
		public ulong OverlayUsedCpuMemory { get; set; }

		public ulong OverlayUsedGpuMemory { get; set; }

		internal void Set(ref PS4MemoryMonitorCallbackInfoSystemMemoryMonitorReportInternal other)
		{
		}
	}
}
