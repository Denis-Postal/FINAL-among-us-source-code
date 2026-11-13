using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyStatByIndexOptionsInternal : ISettable<CopyStatByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_TargetUserId;

		private uint m_StatIndex;

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public uint StatIndex
		{
			set
			{
			}
		}

		public void Set(ref CopyStatByIndexOptions other)
		{
		}

		public void Set(ref CopyStatByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
