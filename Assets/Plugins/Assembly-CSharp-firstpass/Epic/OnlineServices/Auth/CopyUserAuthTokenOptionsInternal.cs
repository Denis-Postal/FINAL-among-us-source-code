using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct CopyUserAuthTokenOptionsInternal : ISettable<CopyUserAuthTokenOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref CopyUserAuthTokenOptions other)
		{
		}

		public void Set(ref CopyUserAuthTokenOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
