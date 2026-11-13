using System;

namespace Epic.OnlineServices.Connect
{
	public sealed class ConnectInterface : Handle
	{
		public const int AddnotifyauthexpirationApiLatest = 1;

		public const int AddnotifyloginstatuschangedApiLatest = 1;

		public const int CopyidtokenApiLatest = 1;

		public const int CopyproductuserexternalaccountbyaccountidApiLatest = 1;

		public const int CopyproductuserexternalaccountbyaccounttypeApiLatest = 1;

		public const int CopyproductuserexternalaccountbyindexApiLatest = 1;

		public const int CopyproductuserinfoApiLatest = 1;

		public const int CreatedeviceidApiLatest = 1;

		public const int CreatedeviceidDevicemodelMaxLength = 64;

		public const int CreateuserApiLatest = 1;

		public const int CredentialsApiLatest = 1;

		public const int DeletedeviceidApiLatest = 1;

		public const int ExternalAccountIdMaxLength = 256;

		public const int ExternalaccountinfoApiLatest = 1;

		public const int GetexternalaccountmappingApiLatest = 1;

		public const int GetexternalaccountmappingsApiLatest = 1;

		public const int GetproductuserexternalaccountcountApiLatest = 1;

		public const int GetproductuseridmappingApiLatest = 1;

		public const int IdtokenApiLatest = 1;

		public const int LinkaccountApiLatest = 1;

		public const int LoginApiLatest = 2;

		public const int LogoutApiLatest = 1;

		public const int OnauthexpirationcallbackApiLatest = 1;

		public const int QueryexternalaccountmappingsApiLatest = 1;

		public const int QueryexternalaccountmappingsMaxAccountIds = 128;

		public const int QueryproductuseridmappingsApiLatest = 2;

		public const int TimeUndefined = -1;

		public const int TransferdeviceidaccountApiLatest = 1;

		public const int UnlinkaccountApiLatest = 1;

		public const int UserlogininfoApiLatest = 2;

		public const int UserlogininfoDisplaynameMaxLength = 32;

		public const int VerifyidtokenApiLatest = 1;

		public ConnectInterface()
		{
		}

		public ConnectInterface(IntPtr innerHandle)
		{
		}

		public ulong AddNotifyAuthExpiration(ref AddNotifyAuthExpirationOptions options, object clientData, OnAuthExpirationCallback notification)
		{
			return 0uL;
		}

		public ulong AddNotifyLoginStatusChanged(ref AddNotifyLoginStatusChangedOptions options, object clientData, OnLoginStatusChangedCallback notification)
		{
			return 0uL;
		}

		public Result CopyIdToken(ref CopyIdTokenOptions options, out IdToken? outIdToken)
		{
			outIdToken = null;
			return Result.Success;
		}

		public Result CopyProductUserExternalAccountByAccountId(ref CopyProductUserExternalAccountByAccountIdOptions options, out ExternalAccountInfo? outExternalAccountInfo)
		{
			outExternalAccountInfo = null;
			return Result.Success;
		}

		public Result CopyProductUserExternalAccountByAccountType(ref CopyProductUserExternalAccountByAccountTypeOptions options, out ExternalAccountInfo? outExternalAccountInfo)
		{
			outExternalAccountInfo = null;
			return Result.Success;
		}

		public Result CopyProductUserExternalAccountByIndex(ref CopyProductUserExternalAccountByIndexOptions options, out ExternalAccountInfo? outExternalAccountInfo)
		{
			outExternalAccountInfo = null;
			return Result.Success;
		}

		public Result CopyProductUserInfo(ref CopyProductUserInfoOptions options, out ExternalAccountInfo? outExternalAccountInfo)
		{
			outExternalAccountInfo = null;
			return Result.Success;
		}

		public void CreateDeviceId(ref CreateDeviceIdOptions options, object clientData, OnCreateDeviceIdCallback completionDelegate)
		{
			if (completionDelegate != null)
			{
				CreateDeviceIdCallbackInfo data = new CreateDeviceIdCallbackInfo
				{
					ResultCode = Result.Success,
					ClientData = clientData
				};
				completionDelegate(ref data);
			}
		}

		public void CreateUser(ref CreateUserOptions options, object clientData, OnCreateUserCallback completionDelegate)
		{
			if (completionDelegate != null)
			{
				CreateUserCallbackInfo data = new CreateUserCallbackInfo
				{
					ResultCode = Result.Success,
					ClientData = clientData,
					LocalUserId = new ProductUserId((IntPtr)1)
				};
				completionDelegate(ref data);
			}
		}

		public void DeleteDeviceId(ref DeleteDeviceIdOptions options, object clientData, OnDeleteDeviceIdCallback completionDelegate)
		{
			if (completionDelegate != null)
			{
				DeleteDeviceIdCallbackInfo data = new DeleteDeviceIdCallbackInfo
				{
					ResultCode = Result.Success,
					ClientData = clientData
				};
				completionDelegate(ref data);
			}
		}

		public ProductUserId GetExternalAccountMapping(ref GetExternalAccountMappingsOptions options)
		{
			return null;
		}

		public ProductUserId GetLoggedInUserByIndex(int index)
		{
			return null;
		}

		public int GetLoggedInUsersCount()
		{
			return 1;
		}

		public LoginStatus GetLoginStatus(ProductUserId localUserId)
		{
			return LoginStatus.LoggedIn;
		}

		public uint GetProductUserExternalAccountCount(ref GetProductUserExternalAccountCountOptions options)
		{
			return 0u;
		}

		public Result GetProductUserIdMapping(ref GetProductUserIdMappingOptions options, out Utf8String outBuffer)
		{
			outBuffer = null;
			return Result.Success;
		}

		public void LinkAccount(ref LinkAccountOptions options, object clientData, OnLinkAccountCallback completionDelegate)
		{
			if (completionDelegate != null)
			{
				LinkAccountCallbackInfo data = new LinkAccountCallbackInfo
				{
					ResultCode = Result.Success,
					ClientData = clientData,
					LocalUserId = new ProductUserId((IntPtr)1)
				};
				completionDelegate(ref data);
			}
		}

		public void Login(ref LoginOptions options, object clientData, OnLoginCallback completionDelegate)
		{
			if (completionDelegate != null)
			{
				LoginCallbackInfo data = new LoginCallbackInfo
				{
					ResultCode = Result.Success,
					ClientData = clientData,
					LocalUserId = new ProductUserId((IntPtr)1)
				};
				completionDelegate(ref data);
			}
		}

		public void Logout(ref LogoutOptions options, object clientData, OnLogoutCallback completionDelegate)
		{
			if (completionDelegate != null)
			{
				LogoutCallbackInfo data = new LogoutCallbackInfo
				{
					ResultCode = Result.Success,
					ClientData = clientData,
					LocalUserId = new ProductUserId((IntPtr)1)
				};
				completionDelegate(ref data);
			}
		}

		public void QueryExternalAccountMappings(ref QueryExternalAccountMappingsOptions options, object clientData, OnQueryExternalAccountMappingsCallback completionDelegate)
		{
		}

		public void QueryProductUserIdMappings(ref QueryProductUserIdMappingsOptions options, object clientData, OnQueryProductUserIdMappingsCallback completionDelegate)
		{
		}

		public void RemoveNotifyAuthExpiration(ulong inId)
		{
		}

		public void RemoveNotifyLoginStatusChanged(ulong inId)
		{
		}

		public void TransferDeviceIdAccount(ref TransferDeviceIdAccountOptions options, object clientData, OnTransferDeviceIdAccountCallback completionDelegate)
		{
		}

		public void UnlinkAccount(ref UnlinkAccountOptions options, object clientData, OnUnlinkAccountCallback completionDelegate)
		{
		}

		public void VerifyIdToken(ref VerifyIdTokenOptions options, object clientData, OnVerifyIdTokenCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnAuthExpirationCallbackInternal))]
		internal static void OnAuthExpirationCallbackInternalImplementation(ref AuthExpirationCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnCreateDeviceIdCallbackInternal))]
		internal static void OnCreateDeviceIdCallbackInternalImplementation(ref CreateDeviceIdCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnCreateUserCallbackInternal))]
		internal static void OnCreateUserCallbackInternalImplementation(ref CreateUserCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnDeleteDeviceIdCallbackInternal))]
		internal static void OnDeleteDeviceIdCallbackInternalImplementation(ref DeleteDeviceIdCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnLinkAccountCallbackInternal))]
		internal static void OnLinkAccountCallbackInternalImplementation(ref LinkAccountCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnLoginCallbackInternal))]
		internal static void OnLoginCallbackInternalImplementation(ref LoginCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnLoginStatusChangedCallbackInternal))]
		internal static void OnLoginStatusChangedCallbackInternalImplementation(ref LoginStatusChangedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnLogoutCallbackInternal))]
		internal static void OnLogoutCallbackInternalImplementation(ref LogoutCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryExternalAccountMappingsCallbackInternal))]
		internal static void OnQueryExternalAccountMappingsCallbackInternalImplementation(ref QueryExternalAccountMappingsCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryProductUserIdMappingsCallbackInternal))]
		internal static void OnQueryProductUserIdMappingsCallbackInternalImplementation(ref QueryProductUserIdMappingsCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnTransferDeviceIdAccountCallbackInternal))]
		internal static void OnTransferDeviceIdAccountCallbackInternalImplementation(ref TransferDeviceIdAccountCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUnlinkAccountCallbackInternal))]
		internal static void OnUnlinkAccountCallbackInternalImplementation(ref UnlinkAccountCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnVerifyIdTokenCallbackInternal))]
		internal static void OnVerifyIdTokenCallbackInternalImplementation(ref VerifyIdTokenCallbackInfoInternal data)
		{
		}
	}
}
