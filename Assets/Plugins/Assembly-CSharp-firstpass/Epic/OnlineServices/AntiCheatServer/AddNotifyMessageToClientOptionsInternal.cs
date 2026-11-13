using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyMessageToClientOptionsInternal : ISettable<AddNotifyMessageToClientOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyMessageToClientOptions other)
		{
		}

		public void Set(ref AddNotifyMessageToClientOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
