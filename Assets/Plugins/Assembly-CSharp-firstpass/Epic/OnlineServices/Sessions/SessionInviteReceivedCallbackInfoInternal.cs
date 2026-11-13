using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct SessionInviteReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<SessionInviteReceivedCallbackInfo>, ISettable<SessionInviteReceivedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

		private IntPtr m_InviteId;

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

		public void Set(ref SessionInviteReceivedCallbackInfo other)
		{
		}

		public void Set(ref SessionInviteReceivedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SessionInviteReceivedCallbackInfo output)
		{
			output = default;
		}
	}
}
