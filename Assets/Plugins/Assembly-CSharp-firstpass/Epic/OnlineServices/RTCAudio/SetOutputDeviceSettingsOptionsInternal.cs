using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct SetOutputDeviceSettingsOptionsInternal : ISettable<SetOutputDeviceSettingsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RealDeviceId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String RealDeviceId
		{
			set
			{
			}
		}

		public void Set(ref SetOutputDeviceSettingsOptions other)
		{
		}

		public void Set(ref SetOutputDeviceSettingsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
