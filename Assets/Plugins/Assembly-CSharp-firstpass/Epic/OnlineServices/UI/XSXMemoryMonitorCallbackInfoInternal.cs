using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct XSXMemoryMonitorCallbackInfoInternal : ICallbackInfoInternal, IGettable<XSXMemoryMonitorCallbackInfo>, ISettable<XSXMemoryMonitorCallbackInfo>, IDisposable
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

		public XSXMemoryMonitorCallbackInfoSystemMemoryMonitorReport? SystemMemoryMonitorReport
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref XSXMemoryMonitorCallbackInfo other)
		{
		}

		public void Set(ref XSXMemoryMonitorCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XSXMemoryMonitorCallbackInfo output)
		{
			output = default;
		}
	}
}
