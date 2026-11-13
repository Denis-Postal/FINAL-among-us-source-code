using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct LobbyInviteReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LobbyInviteReceivedCallbackInfo>, ISettable<LobbyInviteReceivedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_InviteId;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

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

		public void Set(ref LobbyInviteReceivedCallbackInfo other)
		{
		}

		public void Set(ref LobbyInviteReceivedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LobbyInviteReceivedCallbackInfo output)
		{
			output = default;
		}
	}
}
