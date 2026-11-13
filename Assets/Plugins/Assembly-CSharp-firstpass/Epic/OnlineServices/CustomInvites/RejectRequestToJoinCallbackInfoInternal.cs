using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct RejectRequestToJoinCallbackInfoInternal : ICallbackInfoInternal, IGettable<RejectRequestToJoinCallbackInfo>, ISettable<RejectRequestToJoinCallbackInfo>, IDisposable
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

		public ProductUserId TargetUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref RejectRequestToJoinCallbackInfo other)
		{
		}

		public void Set(ref RejectRequestToJoinCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out RejectRequestToJoinCallbackInfo output)
		{
			output = default;
		}
	}
}
