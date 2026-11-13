using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct OnPeerConnectionEstablishedInfoInternal : ICallbackInfoInternal, IGettable<OnPeerConnectionEstablishedInfo>, ISettable<OnPeerConnectionEstablishedInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_RemoteUserId;

		private IntPtr m_SocketId;

		private ConnectionEstablishedType m_ConnectionType;

		private NetworkConnectionType m_NetworkType;

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

		public ConnectionEstablishedType ConnectionType
		{
			get
			{
				return ConnectionEstablishedType.NewConnection;
			}
			set
			{
			}
		}

		public NetworkConnectionType NetworkType
		{
			get
			{
				return NetworkConnectionType.NoConnection;
			}
			set
			{
			}
		}

		public void Set(ref OnPeerConnectionEstablishedInfo other)
		{
		}

		public void Set(ref OnPeerConnectionEstablishedInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnPeerConnectionEstablishedInfo output)
		{
			output = default;
		}
	}
}
