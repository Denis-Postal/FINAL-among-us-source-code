using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct SessionDetailsCopyInfoOptionsInternal : ISettable<SessionDetailsCopyInfoOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref SessionDetailsCopyInfoOptions other)
		{
		}

		public void Set(ref SessionDetailsCopyInfoOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
