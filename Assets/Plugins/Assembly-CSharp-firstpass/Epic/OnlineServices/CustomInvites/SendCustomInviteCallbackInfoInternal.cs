using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct SendCustomInviteCallbackInfoInternal : ICallbackInfoInternal, IGettable<SendCustomInviteCallbackInfo>, ISettable<SendCustomInviteCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserIds;

		private uint m_TargetUserIdsCount;

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

		public ProductUserId LocalUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ProductUserId[] TargetUserIds
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref SendCustomInviteCallbackInfo other)
		{
		}

		public void Set(ref SendCustomInviteCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SendCustomInviteCallbackInfo output)
		{
			output = default;
		}
	}
}
