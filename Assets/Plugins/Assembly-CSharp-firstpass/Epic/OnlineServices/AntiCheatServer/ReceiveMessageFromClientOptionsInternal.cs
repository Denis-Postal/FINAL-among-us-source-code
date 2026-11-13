using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct ReceiveMessageFromClientOptionsInternal : ISettable<ReceiveMessageFromClientOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ClientHandle;

		private uint m_DataLengthBytes;

		private IntPtr m_Data;

		public IntPtr ClientHandle
		{
			set
			{
			}
		}

		public ArraySegment<byte> Data
		{
			set
			{
			}
		}

		public void Set(ref ReceiveMessageFromClientOptions other)
		{
		}

		public void Set(ref ReceiveMessageFromClientOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
