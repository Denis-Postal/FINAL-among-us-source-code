using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct PS5IntegratedPlatformOptionsContainerAddOptionsInternal : ISettable<PS5IntegratedPlatformOptionsContainerAddOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Options;

		public PS5Options? Options
		{
			set
			{
			}
		}

		public void Set(ref PS5IntegratedPlatformOptionsContainerAddOptions other)
		{
		}

		public void Set(ref PS5IntegratedPlatformOptionsContainerAddOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
