using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct XboxOneGDKOptionsSystemSpecificOptionsInternal : IGettable<XboxOneGDKOptionsSystemSpecificOptions>, ISettable<XboxOneGDKOptionsSystemSpecificOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_OverlayGraphicsDevice;

		public IntPtr OverlayGraphicsDevice
		{
			get
			{
				return (IntPtr)0;
			}
			set
			{
			}
		}

		public void Set(ref XboxOneGDKOptionsSystemSpecificOptions other)
		{
		}

		public void Set(ref XboxOneGDKOptionsSystemSpecificOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XboxOneGDKOptionsSystemSpecificOptions output)
		{
			output = default;
		}
	}
}
