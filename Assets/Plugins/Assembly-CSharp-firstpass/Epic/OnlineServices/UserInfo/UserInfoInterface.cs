using System;

namespace Epic.OnlineServices.UserInfo
{
	public sealed class UserInfoInterface : Handle
	{
		public const int BestdisplaynameApiLatest = 1;

		public const int CopybestdisplaynameApiLatest = 1;

		public const int CopybestdisplaynamewithplatformApiLatest = 1;

		public const int CopyexternaluserinfobyaccountidApiLatest = 1;

		public const int CopyexternaluserinfobyaccounttypeApiLatest = 1;

		public const int CopyexternaluserinfobyindexApiLatest = 1;

		public const int CopyuserinfoApiLatest = 3;

		public const int ExternaluserinfoApiLatest = 2;

		public const int GetexternaluserinfocountApiLatest = 1;

		public const int GetlocalplatformtypeApiLatest = 1;

		public const int MaxDisplaynameCharacters = 16;

		public const int MaxDisplaynameUtf8Length = 64;

		public const int QueryuserinfoApiLatest = 1;

		public const int QueryuserinfobydisplaynameApiLatest = 1;

		public const int QueryuserinfobyexternalaccountApiLatest = 1;

		public UserInfoInterface()
		{
		}

		public UserInfoInterface(IntPtr innerHandle)
		{
		}

		public Result CopyBestDisplayName(ref CopyBestDisplayNameOptions options, out BestDisplayName? outBestDisplayName)
		{
			outBestDisplayName = null;
			return Result.Success;
		}

		public Result CopyBestDisplayNameWithPlatform(ref CopyBestDisplayNameWithPlatformOptions options, out BestDisplayName? outBestDisplayName)
		{
			outBestDisplayName = null;
			return Result.Success;
		}

		public Result CopyExternalUserInfoByAccountId(ref CopyExternalUserInfoByAccountIdOptions options, out ExternalUserInfo? outExternalUserInfo)
		{
			outExternalUserInfo = null;
			return Result.Success;
		}

		public Result CopyExternalUserInfoByAccountType(ref CopyExternalUserInfoByAccountTypeOptions options, out ExternalUserInfo? outExternalUserInfo)
		{
			outExternalUserInfo = null;
			return Result.Success;
		}

		public Result CopyExternalUserInfoByIndex(ref CopyExternalUserInfoByIndexOptions options, out ExternalUserInfo? outExternalUserInfo)
		{
			outExternalUserInfo = null;
			return Result.Success;
		}

		public Result CopyUserInfo(ref CopyUserInfoOptions options, out UserInfoData? outUserInfo)
		{
			outUserInfo = null;
			return Result.Success;
		}

		public uint GetExternalUserInfoCount(ref GetExternalUserInfoCountOptions options)
		{
			return 0u;
		}

		public uint GetLocalPlatformType(ref GetLocalPlatformTypeOptions options)
		{
			return 0u;
		}

		public void QueryUserInfo(ref QueryUserInfoOptions options, object clientData, OnQueryUserInfoCallback completionDelegate)
		{
		}

		public void QueryUserInfoByDisplayName(ref QueryUserInfoByDisplayNameOptions options, object clientData, OnQueryUserInfoByDisplayNameCallback completionDelegate)
		{
		}

		public void QueryUserInfoByExternalAccount(ref QueryUserInfoByExternalAccountOptions options, object clientData, OnQueryUserInfoByExternalAccountCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryUserInfoByDisplayNameCallbackInternal))]
		internal static void OnQueryUserInfoByDisplayNameCallbackInternalImplementation(ref QueryUserInfoByDisplayNameCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryUserInfoByExternalAccountCallbackInternal))]
		internal static void OnQueryUserInfoByExternalAccountCallbackInternalImplementation(ref QueryUserInfoByExternalAccountCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryUserInfoCallbackInternal))]
		internal static void OnQueryUserInfoCallbackInternalImplementation(ref QueryUserInfoCallbackInfoInternal data)
		{
		}
	}
}
