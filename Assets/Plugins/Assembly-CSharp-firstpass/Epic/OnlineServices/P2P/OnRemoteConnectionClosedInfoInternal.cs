using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct OnRemoteConnectionClosedInfoInternal : ICallbackInfoInternal, IGettable<OnRemoteConnectionClosedInfo>, ISettable<OnRemoteConnectionClosedInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_RemoteUserId;

		private IntPtr m_SocketId;

		private ConnectionClosedReason m_Reason;

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

		public ProductUserId RemoteUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public SocketId? SocketId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ConnectionClosedReason Reason
		{
			get
			{
				return ConnectionClosedReason.Unknown;
			}
			set
			{
			}
		}

		public void Set(ref OnRemoteConnectionClosedInfo other)
		{
		}

		public void Set(ref OnRemoteConnectionClosedInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnRemoteConnectionClosedInfo output)
		{
			output = default;
		}
	}
}
