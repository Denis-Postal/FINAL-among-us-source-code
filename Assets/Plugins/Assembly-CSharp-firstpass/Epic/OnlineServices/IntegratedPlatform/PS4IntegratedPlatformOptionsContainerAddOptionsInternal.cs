using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct PS4IntegratedPlatformOptionsContainerAddOptionsInternal : ISettable<PS4IntegratedPlatformOptionsContainerAddOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Options;

		public PS4Options? Options
		{
			set
			{
			}
		}

		public void Set(ref PS4IntegratedPlatformOptionsContainerAddOptions other)
		{
		}

		public void Set(ref PS4IntegratedPlatformOptionsContainerAddOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
