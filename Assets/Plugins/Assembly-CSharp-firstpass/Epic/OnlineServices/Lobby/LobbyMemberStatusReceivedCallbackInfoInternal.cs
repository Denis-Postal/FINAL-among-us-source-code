using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct LobbyMemberStatusReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LobbyMemberStatusReceivedCallbackInfo>, ISettable<LobbyMemberStatusReceivedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LobbyId;

		private IntPtr m_TargetUserId;

		private LobbyMemberStatus m_CurrentStatus;

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

		public LobbyMemberStatus CurrentStatus
		{
			get
			{
				return LobbyMemberStatus.Joined;
			}
			set
			{
			}
		}

		public void Set(ref LobbyMemberStatusReceivedCallbackInfo other)
		{
		}

		public void Set(ref LobbyMemberStatusReceivedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LobbyMemberStatusReceivedCallbackInfo output)
		{
			output = default;
		}
	}
}
