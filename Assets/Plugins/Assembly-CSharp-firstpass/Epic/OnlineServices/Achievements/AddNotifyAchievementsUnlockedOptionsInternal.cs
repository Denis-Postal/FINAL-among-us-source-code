using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyAchievementsUnlockedOptionsInternal : ISettable<AddNotifyAchievementsUnlockedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyAchievementsUnlockedOptions other)
		{
		}

		public void Set(ref AddNotifyAchievementsUnlockedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
