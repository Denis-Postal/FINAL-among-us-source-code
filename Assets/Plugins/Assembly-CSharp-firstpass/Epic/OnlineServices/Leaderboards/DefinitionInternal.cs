using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Leaderboards
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct DefinitionInternal : IGettable<Definition>, ISettable<Definition>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LeaderboardId;

		private IntPtr m_StatName;

		private LeaderboardAggregation m_Aggregation;

		private long m_StartTime;

		private long m_EndTime;

		public Utf8String LeaderboardId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String StatName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public LeaderboardAggregation Aggregation
		{
			get
			{
				return LeaderboardAggregation.Min;
			}
			set
			{
			}
		}

		public DateTimeOffset? StartTime
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public DateTimeOffset? EndTime
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref Definition other)
		{
		}

		public void Set(ref Definition? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out Definition output)
		{
			output = default;
		}
	}
}
