using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct CopyAchievementDefinitionV2ByAchievementIdOptionsInternal : ISettable<CopyAchievementDefinitionV2ByAchievementIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_AchievementId;

		public Utf8String AchievementId
		{
			set
			{
			}
		}

		public void Set(ref CopyAchievementDefinitionV2ByAchievementIdOptions other)
		{
		}

		public void Set(ref CopyAchievementDefinitionV2ByAchievementIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
