using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct ProtectMessageOptionsInternal : ISettable<ProtectMessageOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ClientHandle;

		private uint m_DataLengthBytes;

		private IntPtr m_Data;

		private uint m_OutBufferSizeBytes;

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

		public uint OutBufferSizeBytes
		{
			set
			{
			}
		}

		public void Set(ref ProtectMessageOptions other)
		{
		}

		public void Set(ref ProtectMessageOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
