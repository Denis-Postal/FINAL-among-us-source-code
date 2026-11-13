using System;

namespace Epic.OnlineServices.Lobby
{
	public sealed class LobbyModification : Handle
	{
		public const int LobbymodificationAddattributeApiLatest = 2;

		public const int LobbymodificationAddmemberattributeApiLatest = 2;

		public const int LobbymodificationMaxAttributeLength = 64;

		public const int LobbymodificationMaxAttributes = 64;

		public const int LobbymodificationRemoveattributeApiLatest = 1;

		public const int LobbymodificationRemovememberattributeApiLatest = 1;

		public const int LobbymodificationSetallowedplatformidsApiLatest = 1;

		public const int LobbymodificationSetbucketidApiLatest = 1;

		public const int LobbymodificationSetinvitesallowedApiLatest = 1;

		public const int LobbymodificationSetmaxmembersApiLatest = 1;

		public const int LobbymodificationSetpermissionlevelApiLatest = 1;

		public LobbyModification()
		{
		}

		public LobbyModification(IntPtr innerHandle)
		{
		}

		public Result AddAttribute(ref LobbyModificationAddAttributeOptions options)
		{
			return Result.Success;
		}

		public Result AddMemberAttribute(ref LobbyModificationAddMemberAttributeOptions options)
		{
			return Result.Success;
		}

		public void Release()
		{
		}

		public Result RemoveAttribute(ref LobbyModificationRemoveAttributeOptions options)
		{
			return Result.Success;
		}

		public Result RemoveMemberAttribute(ref LobbyModificationRemoveMemberAttributeOptions options)
		{
			return Result.Success;
		}

		public Result SetAllowedPlatformIds(ref LobbyModificationSetAllowedPlatformIdsOptions options)
		{
			return Result.Success;
		}

		public Result SetBucketId(ref LobbyModificationSetBucketIdOptions options)
		{
			return Result.Success;
		}

		public Result SetInvitesAllowed(ref LobbyModificationSetInvitesAllowedOptions options)
		{
			return Result.Success;
		}

		public Result SetMaxMembers(ref LobbyModificationSetMaxMembersOptions options)
		{
			return Result.Success;
		}

		public Result SetPermissionLevel(ref LobbyModificationSetPermissionLevelOptions options)
		{
			return Result.Success;
		}
	}
}
