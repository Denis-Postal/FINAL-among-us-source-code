using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct SessionSearchFindOptionsInternal : ISettable<SessionSearchFindOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref SessionSearchFindOptions other)
		{
		}

		public void Set(ref SessionSearchFindOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
