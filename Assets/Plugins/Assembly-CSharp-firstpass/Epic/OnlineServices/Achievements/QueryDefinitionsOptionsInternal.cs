using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct QueryDefinitionsOptionsInternal : ISettable<QueryDefinitionsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_EpicUserId_DEPRECATED;

		private IntPtr m_HiddenAchievementIds_DEPRECATED;

		private uint m_HiddenAchievementsCount_DEPRECATED;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public EpicAccountId EpicUserId_DEPRECATED
		{
			set
			{
			}
		}

		public Utf8String[] HiddenAchievementIds_DEPRECATED
		{
			set
			{
			}
		}

		public void Set(ref QueryDefinitionsOptions other)
		{
		}

		public void Set(ref QueryDefinitionsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
