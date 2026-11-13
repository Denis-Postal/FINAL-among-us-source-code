using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 56)]
	internal struct QueryStatsOptionsInternal : ISettable<QueryStatsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private long m_StartTime;

		private long m_EndTime;

		private IntPtr m_StatNames;

		private uint m_StatNamesCount;

		private IntPtr m_TargetUserId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public DateTimeOffset? StartTime
		{
			set
			{
			}
		}

		public DateTimeOffset? EndTime
		{
			set
			{
			}
		}

		public Utf8String[] StatNames
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

		public void Set(ref QueryStatsOptions other)
		{
		}

		public void Set(ref QueryStatsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
