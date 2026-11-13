using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct SetRoomSettingOptionsInternal : ISettable<SetRoomSettingOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private IntPtr m_SettingName;

		private IntPtr m_SettingValue;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String RoomName
		{
			set
			{
			}
		}

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

		public void Set(ref SetRoomSettingOptions other)
		{
		}

		public void Set(ref SetRoomSettingOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
