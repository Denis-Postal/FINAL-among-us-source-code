using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct CopyAchievementDefinitionV2ByIndexOptionsInternal : ISettable<CopyAchievementDefinitionV2ByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_AchievementIndex;

		public uint AchievementIndex
		{
			set
			{
			}
		}

		public void Set(ref CopyAchievementDefinitionV2ByIndexOptions other)
		{
		}

		public void Set(ref CopyAchievementDefinitionV2ByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
