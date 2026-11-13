using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyUnlockedAchievementByAchievementIdOptionsInternal : ISettable<CopyUnlockedAchievementByAchievementIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_UserId;

		private IntPtr m_AchievementId;

		public ProductUserId UserId
		{
			set
			{
			}
		}

		public Utf8String AchievementId
		{
			set
			{
			}
		}

		public void Set(ref CopyUnlockedAchievementByAchievementIdOptions other)
		{
		}

		public void Set(ref CopyUnlockedAchievementByAchievementIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
