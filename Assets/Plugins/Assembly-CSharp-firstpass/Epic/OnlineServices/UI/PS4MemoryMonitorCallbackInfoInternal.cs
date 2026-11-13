using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct PS4MemoryMonitorCallbackInfoInternal : ICallbackInfoInternal, IGettable<PS4MemoryMonitorCallbackInfo>, ISettable<PS4MemoryMonitorCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_SystemMemoryMonitorReport;

		public object ClientData
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public IntPtr ClientDataAddress => (IntPtr)0;

		public PS4MemoryMonitorCallbackInfoSystemMemoryMonitorReport? SystemMemoryMonitorReport
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref PS4MemoryMonitorCallbackInfo other)
		{
		}

		public void Set(ref PS4MemoryMonitorCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PS4MemoryMonitorCallbackInfo output)
		{
			output = default;
		}
	}
}
