using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct PS4MemoryMonitorCallbackInfoSystemMemoryMonitorReportInternal : IGettable<PS4MemoryMonitorCallbackInfoSystemMemoryMonitorReport>, ISettable<PS4MemoryMonitorCallbackInfoSystemMemoryMonitorReport>, IDisposable
	{
		private int m_ApiVersion;

		private ulong m_OverlayUsedCpuMemory;

		private ulong m_OverlayUsedGpuMemory;

		public ulong OverlayUsedCpuMemory
		{
			get
			{
				return 0uL;
			}
			set
			{
			}
		}

		public ulong OverlayUsedGpuMemory
		{
			get
			{
				return 0uL;
			}
			set
			{
			}
		}

		public void Set(ref PS4MemoryMonitorCallbackInfoSystemMemoryMonitorReport other)
		{
		}

		public void Set(ref PS4MemoryMonitorCallbackInfoSystemMemoryMonitorReport? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PS4MemoryMonitorCallbackInfoSystemMemoryMonitorReport output)
		{
			output = default;
		}
	}
}
