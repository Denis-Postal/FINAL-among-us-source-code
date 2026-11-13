using System;

namespace Epic.OnlineServices.Auth
{
	public sealed class AuthInterface : Handle
	{
		public const int AccountfeaturerestrictedinfoApiLatest = 1;

		public const int AddnotifyloginstatuschangedApiLatest = 1;

		public const int CopyidtokenApiLatest = 1;

		public const int CopyuserauthtokenApiLatest = 1;

		public const int CredentialsApiLatest = 4;

		public const int DeletepersistentauthApiLatest = 2;

		public const int IdtokenApiLatest = 1;

		public const int LinkaccountApiLatest = 1;

		public const int LoginApiLatest = 3;

		public const int LogoutApiLatest = 1;

		public const int PingrantinfoApiLatest = 2;

		public const int QueryidtokenApiLatest = 1;

		public const int TokenApiLatest = 2;

		public const int VerifyidtokenApiLatest = 1;

		public const int VerifyuserauthApiLatest = 1;

		public const int IosCredentialssystemauthcredentialsoptionsApiLatest = 2;

		public AuthInterface()
		{
		}

		public AuthInterface(IntPtr innerHandle)
		{
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

		public Result CopyUserAuthToken(ref CopyUserAuthTokenOptions options, EpicAccountId localUserId, out Token? outUserAuthToken)
		{
			outUserAuthToken = null;
			return Result.Success;
		}

		public void DeletePersistentAuth(ref DeletePersistentAuthOptions options, object clientData, OnDeletePersistentAuthCallback completionDelegate)
		{
		}

		public EpicAccountId GetLoggedInAccountByIndex(int index)
		{
			return null;
		}

		public int GetLoggedInAccountsCount()
		{
			return 0;
		}

		public LoginStatus GetLoginStatus(EpicAccountId localUserId)
		{
			return LoginStatus.NotLoggedIn;
		}

		public EpicAccountId GetMergedAccountByIndex(EpicAccountId localUserId, uint index)
		{
			return null;
		}

		public uint GetMergedAccountsCount(EpicAccountId localUserId)
		{
			return 0u;
		}

		public Result GetSelectedAccountId(EpicAccountId localUserId, out EpicAccountId outSelectedAccountId)
		{
			outSelectedAccountId = null;
			return Result.Success;
		}

		public void LinkAccount(ref LinkAccountOptions options, object clientData, OnLinkAccountCallback completionDelegate)
		{
		}

		public void Login(ref LoginOptions options, object clientData, OnLoginCallback completionDelegate)
		{
		}

		public void Logout(ref LogoutOptions options, object clientData, OnLogoutCallback completionDelegate)
		{
		}

		public void QueryIdToken(ref QueryIdTokenOptions options, object clientData, OnQueryIdTokenCallback completionDelegate)
		{
		}

		public void RemoveNotifyLoginStatusChanged(ulong inId)
		{
		}

		public void VerifyIdToken(ref VerifyIdTokenOptions options, object clientData, OnVerifyIdTokenCallback completionDelegate)
		{
		}

		public void VerifyUserAuth(ref VerifyUserAuthOptions options, object clientData, OnVerifyUserAuthCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnDeletePersistentAuthCallbackInternal))]
		internal static void OnDeletePersistentAuthCallbackInternalImplementation(ref DeletePersistentAuthCallbackInfoInternal data)
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

		[MonoPInvokeCallback(typeof(OnQueryIdTokenCallbackInternal))]
		internal static void OnQueryIdTokenCallbackInternalImplementation(ref QueryIdTokenCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnVerifyIdTokenCallbackInternal))]
		internal static void OnVerifyIdTokenCallbackInternalImplementation(ref VerifyIdTokenCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnVerifyUserAuthCallbackInternal))]
		internal static void OnVerifyUserAuthCallbackInternalImplementation(ref VerifyUserAuthCallbackInfoInternal data)
		{
		}

		public void Login(ref IOSLoginOptions options, object clientData, OnLoginCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(IOSCreateBackgroundSnapshotViewInternal))]
		internal static IntPtr IOSCreateBackgroundSnapshotViewInternalImplementation(IntPtr context)
		{
			return (IntPtr)0;
		}
	}
}
