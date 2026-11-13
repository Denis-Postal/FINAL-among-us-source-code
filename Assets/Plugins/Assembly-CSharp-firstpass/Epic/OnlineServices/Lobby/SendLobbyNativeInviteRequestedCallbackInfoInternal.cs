using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct SendLobbyNativeInviteRequestedCallbackInfoInternal : ICallbackInfoInternal, IGettable<SendLobbyNativeInviteRequestedCallbackInfo>, ISettable<SendLobbyNativeInviteRequestedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private ulong m_UiEventId;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetNativeAccountType;

		private IntPtr m_TargetUserNativeAccountId;

		private IntPtr m_LobbyId;

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

		public Utf8String LobbyId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref SendLobbyNativeInviteRequestedCallbackInfo other)
		{
		}

		public void Set(ref SendLobbyNativeInviteRequestedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SendLobbyNativeInviteRequestedCallbackInfo output)
		{
			output = default;
		}
	}
}
