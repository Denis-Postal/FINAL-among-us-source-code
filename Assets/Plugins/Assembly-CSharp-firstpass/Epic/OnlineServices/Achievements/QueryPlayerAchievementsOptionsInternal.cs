using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct QueryPlayerAchievementsOptionsInternal : ISettable<QueryPlayerAchievementsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_TargetUserId;

		private IntPtr m_LocalUserId;

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref QueryPlayerAchievementsOptions other)
		{
		}

		public void Set(ref QueryPlayerAchievementsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
