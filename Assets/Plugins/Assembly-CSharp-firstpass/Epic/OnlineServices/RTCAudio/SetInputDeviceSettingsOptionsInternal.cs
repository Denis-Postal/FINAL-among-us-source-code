using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct SetInputDeviceSettingsOptionsInternal : ISettable<SetInputDeviceSettingsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RealDeviceId;

		private int m_PlatformAEC;

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

		public bool PlatformAEC
		{
			set
			{
			}
		}

		public void Set(ref SetInputDeviceSettingsOptions other)
		{
		}

		public void Set(ref SetInputDeviceSettingsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
