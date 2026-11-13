using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 80)]
	internal struct PlayerAchievementInternal : IGettable<PlayerAchievement>, ISettable<PlayerAchievement>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_AchievementId;

		private double m_Progress;

		private long m_UnlockTime;

		private int m_StatInfoCount;

		private IntPtr m_StatInfo;

		private IntPtr m_DisplayName;

		private IntPtr m_Description;

		private IntPtr m_IconURL;

		private IntPtr m_FlavorText;

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

		public double Progress
		{
			get
			{
				return 0.0;
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

		public PlayerStatInfo[] StatInfo
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String DisplayName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String Description
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String IconURL
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String FlavorText
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref PlayerAchievement other)
		{
		}

		public void Set(ref PlayerAchievement? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PlayerAchievement output)
		{
			output = default;
		}
	}
}
