using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct IntegratedPlatformOptionsContainerAddOptionsInternal : ISettable<IntegratedPlatformOptionsContainerAddOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Options;

		public Options? Options
		{
			set
			{
			}
		}

		public void Set(ref IntegratedPlatformOptionsContainerAddOptions other)
		{
		}

		public void Set(ref IntegratedPlatformOptionsContainerAddOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
