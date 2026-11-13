using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct UnlockedAchievementInternal : IGettable<UnlockedAchievement>, ISettable<UnlockedAchievement>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_AchievementId;

		private long m_UnlockTime;

		public Utf8String AchievementId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public DateTimeOffset? UnlockTime
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref UnlockedAchievement other)
		{
		}

		public void Set(ref UnlockedAchievement? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out UnlockedAchievement output)
		{
			output = default;
		}
	}
}
