using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct RTCRoomConnectionChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<RTCRoomConnectionChangedCallbackInfo>, ISettable<RTCRoomConnectionChangedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LobbyId;

		private IntPtr m_LocalUserId;

		private int m_IsConnected;

		private Result m_DisconnectReason;

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

		public bool IsConnected
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public Result DisconnectReason
		{
			get
			{
				return Result.Success;
			}
			set
			{
			}
		}

		public void Set(ref RTCRoomConnectionChangedCallbackInfo other)
		{
		}

		public void Set(ref RTCRoomConnectionChangedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out RTCRoomConnectionChangedCallbackInfo output)
		{
			output = default;
		}
	}
}
