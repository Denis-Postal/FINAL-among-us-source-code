using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct OnAchievementsUnlockedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnAchievementsUnlockedCallbackInfo>, ISettable<OnAchievementsUnlockedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_UserId;

		private uint m_AchievementsCount;

		private IntPtr m_AchievementIds;

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

		public Utf8String[] AchievementIds
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref OnAchievementsUnlockedCallbackInfo other)
		{
		}

		public void Set(ref OnAchievementsUnlockedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnAchievementsUnlockedCallbackInfo output)
		{
			output = default;
		}
	}
}
