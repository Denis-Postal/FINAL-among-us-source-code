using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct PS5MemoryMonitorCallbackInfoSystemMemoryMonitorReportInternal : IGettable<PS5MemoryMonitorCallbackInfoSystemMemoryMonitorReport>, ISettable<PS5MemoryMonitorCallbackInfoSystemMemoryMonitorReport>, IDisposable
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

		public void Set(ref PS5MemoryMonitorCallbackInfoSystemMemoryMonitorReport other)
		{
		}

		public void Set(ref PS5MemoryMonitorCallbackInfoSystemMemoryMonitorReport? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PS5MemoryMonitorCallbackInfoSystemMemoryMonitorReport output)
		{
			output = default;
		}
	}
}
