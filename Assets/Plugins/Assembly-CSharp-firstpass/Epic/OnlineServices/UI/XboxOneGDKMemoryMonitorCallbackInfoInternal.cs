using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct XboxOneGDKMemoryMonitorCallbackInfoInternal : ICallbackInfoInternal, IGettable<XboxOneGDKMemoryMonitorCallbackInfo>, ISettable<XboxOneGDKMemoryMonitorCallbackInfo>, IDisposable
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

		public XboxOneGDKMemoryMonitorCallbackInfoSystemMemoryMonitorReport? SystemMemoryMonitorReport
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref XboxOneGDKMemoryMonitorCallbackInfo other)
		{
		}

		public void Set(ref XboxOneGDKMemoryMonitorCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XboxOneGDKMemoryMonitorCallbackInfo output)
		{
			output = default;
		}
	}
}
