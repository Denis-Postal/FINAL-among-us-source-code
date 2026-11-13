using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct IngestStatOptionsInternal : ISettable<IngestStatOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_Stats;

		private uint m_StatsCount;

		private IntPtr m_TargetUserId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public IngestData[] Stats
		{
			set
			{
			}
		}

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public void Set(ref IngestStatOptions other)
		{
		}

		public void Set(ref IngestStatOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
