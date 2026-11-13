using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct OnUnlockAchievementsCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnUnlockAchievementsCompleteCallbackInfo>, ISettable<OnUnlockAchievementsCompleteCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_UserId;

		private uint m_AchievementsCount;

		public Result ResultCode
		{
			get
			{
				return Result.Success;
			}
			set
			{
			}
		}

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

		public uint AchievementsCount
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public void Set(ref OnUnlockAchievementsCompleteCallbackInfo other)
		{
		}

		public void Set(ref OnUnlockAchievementsCompleteCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnUnlockAchievementsCompleteCallbackInfo output)
		{
			output = default;
		}
	}
}
