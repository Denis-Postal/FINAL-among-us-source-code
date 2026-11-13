using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct DeleteDeviceIdOptionsInternal : ISettable<DeleteDeviceIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref DeleteDeviceIdOptions other)
		{
		}

		public void Set(ref DeleteDeviceIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
