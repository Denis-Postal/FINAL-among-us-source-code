using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct SessionModificationSetHostAddressOptionsInternal : ISettable<SessionModificationSetHostAddressOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_HostAddress;

		public Utf8String HostAddress
		{
			set
			{
			}
		}

		public void Set(ref SessionModificationSetHostAddressOptions other)
		{
		}

		public void Set(ref SessionModificationSetHostAddressOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
