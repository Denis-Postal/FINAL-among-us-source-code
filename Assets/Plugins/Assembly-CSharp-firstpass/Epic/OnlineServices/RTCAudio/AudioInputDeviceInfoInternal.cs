using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct AudioInputDeviceInfoInternal : IGettable<AudioInputDeviceInfo>, ISettable<AudioInputDeviceInfo>, IDisposable
	{
		private int m_ApiVersion;

		private int m_DefaultDevice;

		private IntPtr m_DeviceId;

		private IntPtr m_DeviceName;

		public bool DefaultDevice
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public Utf8String DeviceId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String DeviceName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref AudioInputDeviceInfo other)
		{
		}

		public void Set(ref AudioInputDeviceInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out AudioInputDeviceInfo output)
		{
			output = default;
		}
	}
}
