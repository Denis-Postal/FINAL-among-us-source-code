using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct SocketIdInternal : IGettable<SocketId>, ISettable<SocketId>, IDisposable
	{
		private int m_ApiVersion;

		private byte[] m_SocketName;

		public string SocketName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref SocketId other)
		{
		}

		public void Set(ref SocketId? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SocketId output)
		{
			output = default;
		}
	}
}
