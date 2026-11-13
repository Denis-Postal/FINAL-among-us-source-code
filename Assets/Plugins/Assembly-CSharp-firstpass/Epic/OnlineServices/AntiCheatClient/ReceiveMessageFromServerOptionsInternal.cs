using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct ReceiveMessageFromServerOptionsInternal : ISettable<ReceiveMessageFromServerOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_DataLengthBytes;

		private IntPtr m_Data;

		public ArraySegment<byte> Data
		{
			set
			{
			}
		}

		public void Set(ref ReceiveMessageFromServerOptions other)
		{
		}

		public void Set(ref ReceiveMessageFromServerOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
