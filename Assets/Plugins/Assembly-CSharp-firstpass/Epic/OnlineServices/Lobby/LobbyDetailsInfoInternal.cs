using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 88)]
	internal struct LobbyDetailsInfoInternal : IGettable<LobbyDetailsInfo>, ISettable<LobbyDetailsInfo>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LobbyId;

		private IntPtr m_LobbyOwnerUserId;

		private LobbyPermissionLevel m_PermissionLevel;

		private uint m_AvailableSlots;

		private uint m_MaxMembers;

		private int m_AllowInvites;

		private IntPtr m_BucketId;

		private int m_AllowHostMigration;

		private int m_RTCRoomEnabled;

		private int m_AllowJoinById;

		private int m_RejoinAfterKickRequiresInvite;

		private int m_PresenceEnabled;

		private IntPtr m_AllowedPlatformIds;

		private uint m_AllowedPlatformIdsCount;

		public Utf8String LobbyId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ProductUserId LobbyOwnerUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public LobbyPermissionLevel PermissionLevel
		{
			get
			{
				return LobbyPermissionLevel.Publicadvertised;
			}
			set
			{
			}
		}

		public uint AvailableSlots
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public uint MaxMembers
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public bool AllowInvites
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

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

		public bool AllowHostMigration
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool RTCRoomEnabled
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool AllowJoinById
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool RejoinAfterKickRequiresInvite
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool PresenceEnabled
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

		public void Set(ref LobbyDetailsInfo other)
		{
		}

		public void Set(ref LobbyDetailsInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LobbyDetailsInfo output)
		{
			output = default;
		}
	}
}
