using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct SetUserPreLogoutCallbackOptionsInternal : ISettable<SetUserPreLogoutCallbackOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref SetUserPreLogoutCallbackOptions other)
		{
		}

		public void Set(ref SetUserPreLogoutCallbackOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
