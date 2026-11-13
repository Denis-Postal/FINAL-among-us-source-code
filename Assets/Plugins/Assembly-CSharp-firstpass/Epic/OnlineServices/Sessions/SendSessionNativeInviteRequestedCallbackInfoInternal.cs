using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct SendSessionNativeInviteRequestedCallbackInfoInternal : ICallbackInfoInternal, IGettable<SendSessionNativeInviteRequestedCallbackInfo>, ISettable<SendSessionNativeInviteRequestedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private ulong m_UiEventId;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetNativeAccountType;

		private IntPtr m_TargetUserNativeAccountId;

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

		public ulong UiEventId
		{
			get
			{
				return 0uL;
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

		public Utf8String TargetNativeAccountType
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String TargetUserNativeAccountId
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

		public void Set(ref SendSessionNativeInviteRequestedCallbackInfo other)
		{
		}

		public void Set(ref SendSessionNativeInviteRequestedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SendSessionNativeInviteRequestedCallbackInfo output)
		{
			output = default;
		}
	}
}
