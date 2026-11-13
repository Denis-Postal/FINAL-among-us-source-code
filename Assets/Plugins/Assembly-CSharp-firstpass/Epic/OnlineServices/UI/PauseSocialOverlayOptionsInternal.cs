using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct PauseSocialOverlayOptionsInternal : ISettable<PauseSocialOverlayOptions>, IDisposable
	{
		private int m_ApiVersion;

		private int m_IsPaused;

		public bool IsPaused
		{
			set
			{
			}
		}

		public void Set(ref PauseSocialOverlayOptions other)
		{
		}

		public void Set(ref PauseSocialOverlayOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
