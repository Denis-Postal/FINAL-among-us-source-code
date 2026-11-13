using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct UnregisterClientOptionsInternal : ISettable<UnregisterClientOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ClientHandle;

		public IntPtr ClientHandle
		{
			set
			{
			}
		}

		public void Set(ref UnregisterClientOptions other)
		{
		}

		public void Set(ref UnregisterClientOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
