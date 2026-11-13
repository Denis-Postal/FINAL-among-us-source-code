using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 88)]
	internal struct DefinitionV2Internal : IGettable<DefinitionV2>, ISettable<DefinitionV2>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_AchievementId;

		private IntPtr m_UnlockedDisplayName;

		private IntPtr m_UnlockedDescription;

		private IntPtr m_LockedDisplayName;

		private IntPtr m_LockedDescription;

		private IntPtr m_FlavorText;

		private IntPtr m_UnlockedIconURL;

		private IntPtr m_LockedIconURL;

		private int m_IsHidden;

		private uint m_StatThresholdsCount;

		private IntPtr m_StatThresholds;

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

		public Utf8String UnlockedDisplayName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String UnlockedDescription
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String LockedDisplayName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String LockedDescription
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

		public Utf8String UnlockedIconURL
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String LockedIconURL
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool IsHidden
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public StatThresholds[] StatThresholds
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref DefinitionV2 other)
		{
		}

		public void Set(ref DefinitionV2? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out DefinitionV2 output)
		{
			output = default;
		}
	}
}
