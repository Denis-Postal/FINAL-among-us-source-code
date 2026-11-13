using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct OnAchievementsUnlockedCallbackV2InfoInternal : ICallbackInfoInternal, IGettable<OnAchievementsUnlockedCallbackV2Info>, ISettable<OnAchievementsUnlockedCallbackV2Info>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_UserId;

		private IntPtr m_AchievementId;

		private long m_UnlockTime;

		public object ClientData
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public IntPtr ClientDataAddress => (IntPtr)0;

		public ProductUserId UserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

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

		public void Set(ref OnAchievementsUnlockedCallbackV2Info other)
		{
		}

		public void Set(ref OnAchievementsUnlockedCallbackV2Info? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnAchievementsUnlockedCallbackV2Info output)
		{
			output = default;
		}
	}
}
