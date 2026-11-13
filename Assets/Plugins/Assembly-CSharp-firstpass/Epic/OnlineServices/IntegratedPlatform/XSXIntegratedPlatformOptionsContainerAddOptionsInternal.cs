using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct XSXIntegratedPlatformOptionsContainerAddOptionsInternal : ISettable<XSXIntegratedPlatformOptionsContainerAddOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Options;

		public XSXXBLOptions? Options
		{
			set
			{
			}
		}

		public void Set(ref XSXIntegratedPlatformOptionsContainerAddOptions other)
		{
		}

		public void Set(ref XSXIntegratedPlatformOptionsContainerAddOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
