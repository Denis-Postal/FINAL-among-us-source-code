using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct UnlockAchievementsOptionsInternal : ISettable<UnlockAchievementsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_UserId;

		private IntPtr m_AchievementIds;

		private uint m_AchievementsCount;

		public ProductUserId UserId
		{
			set
			{
			}
		}

		public Utf8String[] AchievementIds
		{
			set
			{
			}
		}

		public void Set(ref UnlockAchievementsOptions other)
		{
		}

		public void Set(ref UnlockAchievementsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
