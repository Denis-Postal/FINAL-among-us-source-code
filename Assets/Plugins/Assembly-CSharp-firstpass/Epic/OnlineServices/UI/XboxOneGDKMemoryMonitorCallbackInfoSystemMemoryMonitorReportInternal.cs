using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct XboxOneGDKMemoryMonitorCallbackInfoSystemMemoryMonitorReportInternal : IGettable<XboxOneGDKMemoryMonitorCallbackInfoSystemMemoryMonitorReport>, ISettable<XboxOneGDKMemoryMonitorCallbackInfoSystemMemoryMonitorReport>, IDisposable
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

		public void Set(ref XboxOneGDKMemoryMonitorCallbackInfoSystemMemoryMonitorReport other)
		{
		}

		public void Set(ref XboxOneGDKMemoryMonitorCallbackInfoSystemMemoryMonitorReport? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XboxOneGDKMemoryMonitorCallbackInfoSystemMemoryMonitorReport output)
		{
			output = default;
		}
	}
}
