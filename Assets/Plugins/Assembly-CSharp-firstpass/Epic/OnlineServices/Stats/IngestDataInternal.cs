using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct IngestDataInternal : IGettable<IngestData>, ISettable<IngestData>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_StatName;

		private int m_IngestAmount;

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

		public int IngestAmount
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public void Set(ref IngestData other)
		{
		}

		public void Set(ref IngestData? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out IngestData output)
		{
			output = default;
		}
	}
}
