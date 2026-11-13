using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct WindowsRTCOptionsInternal : IGettable<WindowsRTCOptions>, ISettable<WindowsRTCOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PlatformSpecificOptions;

		private RTCBackgroundMode m_BackgroundMode;

		public WindowsRTCOptionsPlatformSpecificOptions? PlatformSpecificOptions
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public RTCBackgroundMode BackgroundMode
		{
			get
			{
				return RTCBackgroundMode.LeaveRooms;
			}
			set
			{
			}
		}

		public void Set(ref WindowsRTCOptions other)
		{
		}

		public void Set(ref WindowsRTCOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out WindowsRTCOptions output)
		{
			output = default;
		}
	}
}
