using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct WindowsRTCOptionsPlatformSpecificOptionsInternal : IGettable<WindowsRTCOptionsPlatformSpecificOptions>, ISettable<WindowsRTCOptionsPlatformSpecificOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_XAudio29DllPath;

		public Utf8String XAudio29DllPath
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref WindowsRTCOptionsPlatformSpecificOptions other)
		{
		}

		public void Set(ref WindowsRTCOptionsPlatformSpecificOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out WindowsRTCOptionsPlatformSpecificOptions output)
		{
			output = default;
		}
	}
}
