using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 88)]
	internal struct CreateLobbyOptionsInternal : ISettable<CreateLobbyOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private uint m_MaxLobbyMembers;

		private LobbyPermissionLevel m_PermissionLevel;

		private int m_PresenceEnabled;

		private int m_AllowInvites;

		private IntPtr m_BucketId;

		private int m_DisableHostMigration;

		private int m_EnableRTCRoom;

		private IntPtr m_LocalRTCOptions;

		private IntPtr m_LobbyId;

		private int m_EnableJoinById;

		private int m_RejoinAfterKickRequiresInvite;

		private IntPtr m_AllowedPlatformIds;

		private uint m_AllowedPlatformIdsCount;

		private int m_CrossplayOptOut;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public uint MaxLobbyMembers
		{
			set
			{
			}
		}

		public LobbyPermissionLevel PermissionLevel
		{
			set
			{
			}
		}

		public bool PresenceEnabled
		{
			set
			{
			}
		}

		public bool AllowInvites
		{
			set
			{
			}
		}

		public Utf8String BucketId
		{
			set
			{
			}
		}

		public bool DisableHostMigration
		{
			set
			{
			}
		}

		public bool EnableRTCRoom
		{
			set
			{
			}
		}

		public LocalRTCOptions? LocalRTCOptions
		{
			set
			{
			}
		}

		public Utf8String LobbyId
		{
			set
			{
			}
		}

		public bool EnableJoinById
		{
			set
			{
			}
		}

		public bool RejoinAfterKickRequiresInvite
		{
			set
			{
			}
		}

		public uint[] AllowedPlatformIds
		{
			set
			{
			}
		}

		public bool CrossplayOptOut
		{
			set
			{
			}
		}

		public void Set(ref CreateLobbyOptions other)
		{
		}

		public void Set(ref CreateLobbyOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
