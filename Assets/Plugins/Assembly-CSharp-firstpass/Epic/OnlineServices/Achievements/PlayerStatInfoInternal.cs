using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct PlayerStatInfoInternal : IGettable<PlayerStatInfo>, ISettable<PlayerStatInfo>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Name;

		private int m_CurrentValue;

		private int m_ThresholdValue;

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

		public int CurrentValue
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public int ThresholdValue
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public void Set(ref PlayerStatInfo other)
		{
		}

		public void Set(ref PlayerStatInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PlayerStatInfo output)
		{
			output = default;
		}
	}
}
