using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct SessionSearchSetSessionIdOptionsInternal : ISettable<SessionSearchSetSessionIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_SessionId;

		public Utf8String SessionId
		{
			set
			{
			}
		}

		public void Set(ref SessionSearchSetSessionIdOptions other)
		{
		}

		public void Set(ref SessionSearchSetSessionIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
