using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct EndSessionOptionsInternal : ISettable<EndSessionOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref EndSessionOptions other)
		{
		}

		public void Set(ref EndSessionOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
