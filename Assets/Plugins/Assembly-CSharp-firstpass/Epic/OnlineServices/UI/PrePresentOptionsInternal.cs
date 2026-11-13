using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct PrePresentOptionsInternal : ISettable<PrePresentOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PlatformSpecificData;

		public IntPtr PlatformSpecificData
		{
			set
			{
			}
		}

		public void Set(ref PrePresentOptions other)
		{
		}

		public void Set(ref PrePresentOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
