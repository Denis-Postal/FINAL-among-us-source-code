using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct SessionSearchCopySearchResultByIndexOptionsInternal : ISettable<SessionSearchCopySearchResultByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_SessionIndex;

		public uint SessionIndex
		{
			set
			{
			}
		}

		public void Set(ref SessionSearchCopySearchResultByIndexOptions other)
		{
		}

		public void Set(ref SessionSearchCopySearchResultByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
