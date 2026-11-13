using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 56)]
	internal struct SessionDetailsSettingsInternal : IGettable<SessionDetailsSettings>, ISettable<SessionDetailsSettings>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_BucketId;

		private uint m_NumPublicConnections;

		private int m_AllowJoinInProgress;

		private OnlineSessionPermissionLevel m_PermissionLevel;

		private int m_InvitesAllowed;

		private int m_SanctionsEnabled;

		private IntPtr m_AllowedPlatformIds;

		private uint m_AllowedPlatformIdsCount;

		public Utf8String BucketId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public uint NumPublicConnections
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public bool AllowJoinInProgress
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public OnlineSessionPermissionLevel PermissionLevel
		{
			get
			{
				return OnlineSessionPermissionLevel.PublicAdvertised;
			}
			set
			{
			}
		}

		public bool InvitesAllowed
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool SanctionsEnabled
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public uint[] AllowedPlatformIds
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref SessionDetailsSettings other)
		{
		}

		public void Set(ref SessionDetailsSettings? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SessionDetailsSettings output)
		{
			output = default;
		}
	}
}
