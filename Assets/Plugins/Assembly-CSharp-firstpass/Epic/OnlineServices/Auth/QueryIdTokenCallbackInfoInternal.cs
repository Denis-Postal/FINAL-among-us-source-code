using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct QueryIdTokenCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryIdTokenCallbackInfo>, ISettable<QueryIdTokenCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetAccountId;

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

		public EpicAccountId TargetAccountId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref QueryIdTokenCallbackInfo other)
		{
		}

		public void Set(ref QueryIdTokenCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out QueryIdTokenCallbackInfo output)
		{
			output = default;
		}
	}
}
