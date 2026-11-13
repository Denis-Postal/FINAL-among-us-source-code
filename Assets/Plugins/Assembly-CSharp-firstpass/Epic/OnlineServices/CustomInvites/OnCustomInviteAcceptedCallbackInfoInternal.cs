using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct OnCustomInviteAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnCustomInviteAcceptedCallbackInfo>, ISettable<OnCustomInviteAcceptedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_TargetUserId;

		private IntPtr m_LocalUserId;

		private IntPtr m_CustomInviteId;

		private IntPtr m_Payload;

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

		public Utf8String CustomInviteId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String Payload
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref OnCustomInviteAcceptedCallbackInfo other)
		{
		}

		public void Set(ref OnCustomInviteAcceptedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnCustomInviteAcceptedCallbackInfo output)
		{
			output = default;
		}
	}
}
