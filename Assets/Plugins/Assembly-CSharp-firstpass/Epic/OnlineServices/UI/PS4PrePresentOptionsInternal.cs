using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct PS4PrePresentOptionsInternal : ISettable<PS4PrePresentOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PlatformSpecificData;

		public PS4PrePresentOptionsPlatformSpecificData? PlatformSpecificData
		{
			set
			{
			}
		}

		public void Set(ref PS4PrePresentOptions other)
		{
		}

		public void Set(ref PS4PrePresentOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
