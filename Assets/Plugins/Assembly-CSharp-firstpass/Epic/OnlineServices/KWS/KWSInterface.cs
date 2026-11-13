using System;

namespace Epic.OnlineServices.KWS
{
	public sealed class KWSInterface : Handle
	{
		public const int AddnotifypermissionsupdatereceivedApiLatest = 1;

		public const int CopypermissionbyindexApiLatest = 1;

		public const int CreateuserApiLatest = 1;

		public const int GetpermissionbykeyApiLatest = 1;

		public const int GetpermissionscountApiLatest = 1;

		public const int MaxPermissionLength = 32;

		public const int MaxPermissions = 16;

		public const int PermissionstatusApiLatest = 1;

		public const int QueryagegateApiLatest = 1;

		public const int QuerypermissionsApiLatest = 1;

		public const int RequestpermissionsApiLatest = 1;

		public const int UpdateparentemailApiLatest = 1;

		public KWSInterface()
		{
		}

		public KWSInterface(IntPtr innerHandle)
		{
		}

		public ulong AddNotifyPermissionsUpdateReceived(ref AddNotifyPermissionsUpdateReceivedOptions options, object clientData, OnPermissionsUpdateReceivedCallback notificationFn)
		{
			return 0uL;
		}

		public Result CopyPermissionByIndex(ref CopyPermissionByIndexOptions options, out PermissionStatus? outPermission)
		{
			outPermission = null;
			return Result.Success;
		}

		public void CreateUser(ref CreateUserOptions options, object clientData, OnCreateUserCallback completionDelegate)
		{
			if (completionDelegate != null)
			{
				CreateUserCallbackInfo data = new CreateUserCallbackInfo
				{
					ResultCode = Result.Success,
					ClientData = clientData,
					LocalUserId = options.LocalUserId
				};
				completionDelegate(ref data);
			}
		}

		public Result GetPermissionByKey(ref GetPermissionByKeyOptions options, out KWSPermissionStatus outPermission)
		{
			outPermission = KWSPermissionStatus.Granted;
			return Result.Success;
		}

		public int GetPermissionsCount(ref GetPermissionsCountOptions options)
		{
			return 4;
		}

		public void QueryAgeGate(ref QueryAgeGateOptions options, object clientData, OnQueryAgeGateCallback completionDelegate)
		{
			if (completionDelegate != null)
			{
				QueryAgeGateCallbackInfo data = new QueryAgeGateCallbackInfo
				{
					ResultCode = Result.Success,
					ClientData = clientData,
					AgeOfConsent = 13,
					CountryCode = "US"
				};
				completionDelegate(ref data);
			}
		}

		public void QueryPermissions(ref QueryPermissionsOptions options, object clientData, OnQueryPermissionsCallback completionDelegate)
		{
			if (completionDelegate != null)
			{
				QueryPermissionsCallbackInfo data = new QueryPermissionsCallbackInfo
				{
					ResultCode = Result.Success,
					ClientData = clientData,
					LocalUserId = options.LocalUserId,
					KWSUserId = "kws_user",
					DateOfBirth = "2000-01-01"
				};
				completionDelegate(ref data);
			}
		}

		public void RemoveNotifyPermissionsUpdateReceived(ulong inId)
		{
		}

		public void RequestPermissions(ref RequestPermissionsOptions options, object clientData, OnRequestPermissionsCallback completionDelegate)
		{
			if (completionDelegate != null)
			{
				RequestPermissionsCallbackInfo data = new RequestPermissionsCallbackInfo
				{
					ResultCode = Result.Success,
					ClientData = clientData,
					LocalUserId = options.LocalUserId
				};
				completionDelegate(ref data);
			}
		}

		public void UpdateParentEmail(ref UpdateParentEmailOptions options, object clientData, OnUpdateParentEmailCallback completionDelegate)
		{
			if (completionDelegate != null)
			{
				UpdateParentEmailCallbackInfo data = new UpdateParentEmailCallbackInfo
				{
					ResultCode = Result.Success,
					ClientData = clientData,
					LocalUserId = options.LocalUserId
				};
				completionDelegate(ref data);
			}
		}

		[MonoPInvokeCallback(typeof(OnCreateUserCallbackInternal))]
		internal static void OnCreateUserCallbackInternalImplementation(ref CreateUserCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnPermissionsUpdateReceivedCallbackInternal))]
		internal static void OnPermissionsUpdateReceivedCallbackInternalImplementation(ref PermissionsUpdateReceivedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryAgeGateCallbackInternal))]
		internal static void OnQueryAgeGateCallbackInternalImplementation(ref QueryAgeGateCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryPermissionsCallbackInternal))]
		internal static void OnQueryPermissionsCallbackInternalImplementation(ref QueryPermissionsCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnRequestPermissionsCallbackInternal))]
		internal static void OnRequestPermissionsCallbackInternalImplementation(ref RequestPermissionsCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUpdateParentEmailCallbackInternal))]
		internal static void OnUpdateParentEmailCallbackInternalImplementation(ref UpdateParentEmailCallbackInfoInternal data)
		{
		}
	}
}
