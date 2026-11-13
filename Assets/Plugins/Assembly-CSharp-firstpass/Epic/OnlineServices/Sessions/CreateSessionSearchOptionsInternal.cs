using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct CreateSessionSearchOptionsInternal : ISettable<CreateSessionSearchOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_MaxSearchResults;

		public uint MaxSearchResults
		{
			set
			{
			}
		}

		public void Set(ref CreateSessionSearchOptions other)
		{
		}

		public void Set(ref CreateSessionSearchOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
