using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct MemoryMonitorCallbackInfoInternal : ICallbackInfoInternal, IGettable<MemoryMonitorCallbackInfo>, ISettable<MemoryMonitorCallbackInfo>, IDisposable
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

		public IntPtr SystemMemoryMonitorReport
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		public void Set(ref MemoryMonitorCallbackInfo other)
		{
		}

		public void Set(ref MemoryMonitorCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out MemoryMonitorCallbackInfo output)
		{
			output = default;
		}
	}
}
