using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct QueryUserInfoByDisplayNameCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryUserInfoByDisplayNameCallbackInfo>, ISettable<QueryUserInfoByDisplayNameCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

		private IntPtr m_DisplayName;

		public Result ResultCode
		{
			get
			{
				return Result.Success;
			}
			set
			{
			}
		}

		public object ClientData
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public IntPtr ClientDataAddress => (IntPtr)0;

		public EpicAccountId LocalUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public EpicAccountId TargetUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String DisplayName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref QueryUserInfoByDisplayNameCallbackInfo other)
		{
		}

		public void Set(ref QueryUserInfoByDisplayNameCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out QueryUserInfoByDisplayNameCallbackInfo output)
		{
			output = default;
		}
	}
}
