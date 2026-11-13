using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyMemoryMonitorOptionsInternal : ISettable<AddNotifyMemoryMonitorOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyMemoryMonitorOptions other)
		{
		}

		public void Set(ref AddNotifyMemoryMonitorOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
