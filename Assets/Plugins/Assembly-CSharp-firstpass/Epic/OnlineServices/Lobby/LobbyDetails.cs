using System;

namespace Epic.OnlineServices.Lobby
{
	public sealed class LobbyDetails : Handle
	{
		public const int LobbydetailsCopyattributebyindexApiLatest = 1;

		public const int LobbydetailsCopyattributebykeyApiLatest = 1;

		public const int LobbydetailsCopyinfoApiLatest = 1;

		public const int LobbydetailsCopymemberattributebyindexApiLatest = 1;

		public const int LobbydetailsCopymemberattributebykeyApiLatest = 1;

		public const int LobbydetailsCopymemberinfoApiLatest = 1;

		public const int LobbydetailsGetattributecountApiLatest = 1;

		public const int LobbydetailsGetlobbyownerApiLatest = 1;

		public const int LobbydetailsGetmemberattributecountApiLatest = 1;

		public const int LobbydetailsGetmemberbyindexApiLatest = 1;

		public const int LobbydetailsGetmembercountApiLatest = 1;

		public const int LobbydetailsInfoApiLatest = 3;

		public const int LobbydetailsMemberinfoApiLatest = 1;

		public LobbyDetails()
		{
		}

		public LobbyDetails(IntPtr innerHandle)
		{
		}

		public Result CopyAttributeByIndex(ref LobbyDetailsCopyAttributeByIndexOptions options, out Attribute? outAttribute)
		{
			outAttribute = null;
			return Result.Success;
		}

		public Result CopyAttributeByKey(ref LobbyDetailsCopyAttributeByKeyOptions options, out Attribute? outAttribute)
		{
			outAttribute = null;
			return Result.Success;
		}

		public Result CopyInfo(ref LobbyDetailsCopyInfoOptions options, out LobbyDetailsInfo? outLobbyDetailsInfo)
		{
			outLobbyDetailsInfo = null;
			return Result.Success;
		}

		public Result CopyMemberAttributeByIndex(ref LobbyDetailsCopyMemberAttributeByIndexOptions options, out Attribute? outAttribute)
		{
			outAttribute = null;
			return Result.Success;
		}

		public Result CopyMemberAttributeByKey(ref LobbyDetailsCopyMemberAttributeByKeyOptions options, out Attribute? outAttribute)
		{
			outAttribute = null;
			return Result.Success;
		}

		public Result CopyMemberInfo(ref LobbyDetailsCopyMemberInfoOptions options, out LobbyDetailsMemberInfo? outLobbyDetailsMemberInfo)
		{
			outLobbyDetailsMemberInfo = null;
			return Result.Success;
		}

		public uint GetAttributeCount(ref LobbyDetailsGetAttributeCountOptions options)
		{
			return 0u;
		}

		public ProductUserId GetLobbyOwner(ref LobbyDetailsGetLobbyOwnerOptions options)
		{
			return null;
		}

		public uint GetMemberAttributeCount(ref LobbyDetailsGetMemberAttributeCountOptions options)
		{
			return 0u;
		}

		public ProductUserId GetMemberByIndex(ref LobbyDetailsGetMemberByIndexOptions options)
		{
			return null;
		}

		public uint GetMemberCount(ref LobbyDetailsGetMemberCountOptions options)
		{
			return 0u;
		}

		public void Release()
		{
		}
	}
}
