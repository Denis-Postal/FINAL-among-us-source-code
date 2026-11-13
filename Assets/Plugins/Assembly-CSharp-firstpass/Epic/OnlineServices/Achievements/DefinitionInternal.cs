using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Achievements
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 96)]
	internal struct DefinitionInternal : IGettable<Definition>, ISettable<Definition>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_AchievementId;

		private IntPtr m_DisplayName;

		private IntPtr m_Description;

		private IntPtr m_LockedDisplayName;

		private IntPtr m_LockedDescription;

		private IntPtr m_HiddenDescription;

		private IntPtr m_CompletionDescription;

		private IntPtr m_UnlockedIconId;

		private IntPtr m_LockedIconId;

		private int m_IsHidden;

		private int m_StatThresholdsCount;

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

		public Utf8String HiddenDescription
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String CompletionDescription
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String UnlockedIconId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String LockedIconId
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

		public void Set(ref Definition other)
		{
		}

		public void Set(ref Definition? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out Definition output)
		{
			output = default;
		}
	}
}
