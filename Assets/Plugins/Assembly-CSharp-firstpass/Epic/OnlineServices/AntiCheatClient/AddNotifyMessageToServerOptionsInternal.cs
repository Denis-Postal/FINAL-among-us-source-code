using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyMessageToServerOptionsInternal : ISettable<AddNotifyMessageToServerOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyMessageToServerOptions other)
		{
		}

		public void Set(ref AddNotifyMessageToServerOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
