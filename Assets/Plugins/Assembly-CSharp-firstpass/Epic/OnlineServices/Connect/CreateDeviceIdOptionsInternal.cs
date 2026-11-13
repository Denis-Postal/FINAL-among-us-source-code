using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct CreateDeviceIdOptionsInternal : ISettable<CreateDeviceIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_DeviceModel;

		public Utf8String DeviceModel
		{
			set
			{
			}
		}

		public void Set(ref CreateDeviceIdOptions other)
		{
		}

		public void Set(ref CreateDeviceIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
