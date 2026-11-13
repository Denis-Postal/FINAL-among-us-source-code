using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct SetSettingOptionsInternal : ISettable<SetSettingOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_SettingName;

		private IntPtr m_SettingValue;

		public Utf8String SettingName
		{
			set
			{
			}
		}

		public Utf8String SettingValue
		{
			set
			{
			}
		}

		public void Set(ref SetSettingOptions other)
		{
		}

		public void Set(ref SetSettingOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
