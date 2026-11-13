using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct XSXPrePresentOptionsInternal : ISettable<XSXPrePresentOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PlatformSpecificData;

		public XSXPrePresentOptionsPlatformSpecificData? PlatformSpecificData
		{
			set
			{
			}
		}

		public void Set(ref XSXPrePresentOptions other)
		{
		}

		public void Set(ref XSXPrePresentOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
