using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct RejectInviteCallbackInfoInternal : ICallbackInfoInternal, IGettable<RejectInviteCallbackInfo>, ISettable<RejectInviteCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_InviteId;

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

		public Utf8String InviteId
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
