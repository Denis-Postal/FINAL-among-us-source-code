using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct SessionInviteRejectedCallbackInfoInternal : ICallbackInfoInternal, IGettable<SessionInviteRejectedCallbackInfo>, ISettable<SessionInviteRejectedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_InviteId;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

		private IntPtr m_SessionId;

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

		public Utf8String SessionId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref SessionInviteRejectedCallbackInfo other)
		{
		}

		public void Set(ref SessionInviteRejectedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SessionInviteRejectedCallbackInfo output)
		{
			output = default;
		}
	}
}
