using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct StatInternal : IGettable<Stat>, ISettable<Stat>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Name;

		private long m_StartTime;

		private long m_EndTime;

		private int m_Value;

		public Utf8String Name
		{
			get
			{
				return null;
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

		public int Value
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public void Set(ref Stat other)
		{
		}

		public void Set(ref Stat? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out Stat output)
		{
			output = default;
		}
	}
}
