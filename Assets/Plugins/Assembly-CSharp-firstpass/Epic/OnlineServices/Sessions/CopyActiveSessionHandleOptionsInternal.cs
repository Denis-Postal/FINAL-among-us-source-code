using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct CopyActiveSessionHandleOptionsInternal : ISettable<CopyActiveSessionHandleOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_SessionName;

		public Utf8String SessionName
		{
			set
			{
			}
		}

		public void Set(ref CopyActiveSessionHandleOptions other)
		{
		}

		public void Set(ref CopyActiveSessionHandleOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
