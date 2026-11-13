using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct GetDesktopCrossplayStatusOptionsInternal : ISettable<GetDesktopCrossplayStatusOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref GetDesktopCrossplayStatusOptions other)
		{
		}

		public void Set(ref GetDesktopCrossplayStatusOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
