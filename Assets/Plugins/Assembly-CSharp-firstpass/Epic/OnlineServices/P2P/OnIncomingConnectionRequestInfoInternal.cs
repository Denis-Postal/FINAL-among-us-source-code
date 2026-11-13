using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct OnIncomingConnectionRequestInfoInternal : ICallbackInfoInternal, IGettable<OnIncomingConnectionRequestInfo>, ISettable<OnIncomingConnectionRequestInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_RemoteUserId;

		private IntPtr m_SocketId;

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

		public void Set(ref OnIncomingConnectionRequestInfo other)
		{
		}

		public void Set(ref OnIncomingConnectionRequestInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnIncomingConnectionRequestInfo output)
		{
			output = default;
		}
	}
}
