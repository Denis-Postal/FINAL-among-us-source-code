using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct StatThresholdsInternal : IGettable<StatThresholds>, ISettable<StatThresholds>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Name;

		private int m_Threshold;

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

		public int Threshold
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public void Set(ref StatThresholds other)
		{
		}

		public void Set(ref StatThresholds? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out StatThresholds output)
		{
			output = default;
		}
	}
}
