using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyClientActionRequiredOptionsInternal : ISettable<AddNotifyClientActionRequiredOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyClientActionRequiredOptions other)
		{
		}

		public void Set(ref AddNotifyClientActionRequiredOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
