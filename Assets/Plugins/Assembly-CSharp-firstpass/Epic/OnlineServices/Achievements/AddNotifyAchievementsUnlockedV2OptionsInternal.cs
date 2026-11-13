using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyAchievementsUnlockedV2OptionsInternal : ISettable<AddNotifyAchievementsUnlockedV2Options>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyAchievementsUnlockedV2Options other)
		{
		}

		public void Set(ref AddNotifyAchievementsUnlockedV2Options? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
