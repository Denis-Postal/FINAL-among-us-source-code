using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct SessionDetailsCopySessionAttributeByKeyOptionsInternal : ISettable<SessionDetailsCopySessionAttributeByKeyOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_AttrKey;

		public Utf8String AttrKey
		{
			set
			{
			}
		}

		public void Set(ref SessionDetailsCopySessionAttributeByKeyOptions other)
		{
		}

		public void Set(ref SessionDetailsCopySessionAttributeByKeyOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
