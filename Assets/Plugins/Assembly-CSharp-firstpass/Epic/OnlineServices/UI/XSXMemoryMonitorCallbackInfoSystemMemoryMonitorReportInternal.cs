using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct XSXMemoryMonitorCallbackInfoSystemMemoryMonitorReportInternal : IGettable<XSXMemoryMonitorCallbackInfoSystemMemoryMonitorReport>, ISettable<XSXMemoryMonitorCallbackInfoSystemMemoryMonitorReport>, IDisposable
	{
		private int m_ApiVersion;

		private ulong m_OverlayUsedCpuMemory;

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

		public void Set(ref XSXMemoryMonitorCallbackInfoSystemMemoryMonitorReport other)
		{
		}

		public void Set(ref XSXMemoryMonitorCallbackInfoSystemMemoryMonitorReport? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XSXMemoryMonitorCallbackInfoSystemMemoryMonitorReport output)
		{
			output = default;
		}
	}
}
