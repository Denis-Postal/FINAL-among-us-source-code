using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct PS5MemoryMonitorCallbackInfoInternal : ICallbackInfoInternal, IGettable<PS5MemoryMonitorCallbackInfo>, ISettable<PS5MemoryMonitorCallbackInfo>, IDisposable
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

		public PS5MemoryMonitorCallbackInfoSystemMemoryMonitorReport? SystemMemoryMonitorReport
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref PS5MemoryMonitorCallbackInfo other)
		{
		}

		public void Set(ref PS5MemoryMonitorCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PS5MemoryMonitorCallbackInfo output)
		{
			output = default;
		}
	}
}
