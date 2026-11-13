using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyPeerActionRequiredOptionsInternal : ISettable<AddNotifyPeerActionRequiredOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyPeerActionRequiredOptions other)
		{
		}

		public void Set(ref AddNotifyPeerActionRequiredOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
