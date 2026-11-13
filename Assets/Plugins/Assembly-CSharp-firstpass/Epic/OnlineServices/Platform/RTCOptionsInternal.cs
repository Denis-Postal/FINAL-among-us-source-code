using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct RTCOptionsInternal : IGettable<RTCOptions>, ISettable<RTCOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PlatformSpecificOptions;

		private RTCBackgroundMode m_BackgroundMode;

		public IntPtr PlatformSpecificOptions
		{
			get
			{
				return (IntPtr)0;
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

		public void Set(ref RTCOptions other)
		{
		}

		public void Set(ref RTCOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out RTCOptions output)
		{
			output = default;
		}
	}
}
