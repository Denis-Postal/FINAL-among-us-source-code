using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct CreateIntegratedPlatformOptionsContainerOptionsInternal : ISettable<CreateIntegratedPlatformOptionsContainerOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref CreateIntegratedPlatformOptionsContainerOptions other)
		{
		}

		public void Set(ref CreateIntegratedPlatformOptionsContainerOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
