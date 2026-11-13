using System;

namespace Epic.OnlineServices.Sessions
{
	public sealed class SessionModification : Handle
	{
		public const int SessionmodificationAddattributeApiLatest = 2;

		public const int SessionmodificationMaxSessionAttributeLength = 64;

		public const int SessionmodificationMaxSessionAttributes = 64;

		public const int SessionmodificationMaxSessionidoverrideLength = 64;

		public const int SessionmodificationMinSessionidoverrideLength = 16;

		public const int SessionmodificationRemoveattributeApiLatest = 1;

		public const int SessionmodificationSetallowedplatformidsApiLatest = 1;

		public const int SessionmodificationSetbucketidApiLatest = 1;

		public const int SessionmodificationSethostaddressApiLatest = 1;

		public const int SessionmodificationSetinvitesallowedApiLatest = 1;

		public const int SessionmodificationSetjoininprogressallowedApiLatest = 1;

		public const int SessionmodificationSetmaxplayersApiLatest = 1;

		public const int SessionmodificationSetpermissionlevelApiLatest = 1;

		public SessionModification()
		{
		}

		public SessionModification(IntPtr innerHandle)
		{
		}

		public Result AddAttribute(ref SessionModificationAddAttributeOptions options)
		{
			return Result.Success;
		}

		public void Release()
		{
		}

		public Result RemoveAttribute(ref SessionModificationRemoveAttributeOptions options)
		{
			return Result.Success;
		}

		public Result SetAllowedPlatformIds(ref SessionModificationSetAllowedPlatformIdsOptions options)
		{
			return Result.Success;
		}

		public Result SetBucketId(ref SessionModificationSetBucketIdOptions options)
		{
			return Result.Success;
		}

		public Result SetHostAddress(ref SessionModificationSetHostAddressOptions options)
		{
			return Result.Success;
		}

		public Result SetInvitesAllowed(ref SessionModificationSetInvitesAllowedOptions options)
		{
			return Result.Success;
		}

		public Result SetJoinInProgressAllowed(ref SessionModificationSetJoinInProgressAllowedOptions options)
		{
			return Result.Success;
		}

		public Result SetMaxPlayers(ref SessionModificationSetMaxPlayersOptions options)
		{
			return Result.Success;
		}

		public Result SetPermissionLevel(ref SessionModificationSetPermissionLevelOptions options)
		{
			return Result.Success;
		}
	}
}
