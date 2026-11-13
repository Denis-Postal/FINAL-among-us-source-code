using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct SetAudioInputSettingsOptionsInternal : ISettable<SetAudioInputSettingsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_DeviceId;

		private float m_Volume;

		private int m_PlatformAEC;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String DeviceId
		{
			set
			{
			}
		}

		public float Volume
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

		public void Set(ref SetAudioInputSettingsOptions other)
		{
		}

		public void Set(ref SetAudioInputSettingsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
