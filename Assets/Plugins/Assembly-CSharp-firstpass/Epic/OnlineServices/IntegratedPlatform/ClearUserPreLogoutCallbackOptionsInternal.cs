using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct ClearUserPreLogoutCallbackOptionsInternal : ISettable<ClearUserPreLogoutCallbackOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref ClearUserPreLogoutCallbackOptions other)
		{
		}

		public void Set(ref ClearUserPreLogoutCallbackOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
