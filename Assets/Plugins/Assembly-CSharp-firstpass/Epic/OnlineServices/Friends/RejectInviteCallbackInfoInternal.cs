using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct RejectInviteCallbackInfoInternal : ICallbackInfoInternal, IGettable<RejectInviteCallbackInfo>, ISettable<RejectInviteCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

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

		public void Set(ref RejectInviteCallbackInfo other)
		{
		}

		public void Set(ref RejectInviteCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out RejectInviteCallbackInfo output)
		{
			output = default;
		}
	}
}
