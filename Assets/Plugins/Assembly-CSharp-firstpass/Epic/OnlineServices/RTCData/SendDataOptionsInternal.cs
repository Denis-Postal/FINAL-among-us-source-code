using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct SendDataOptionsInternal : ISettable<SendDataOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private uint m_DataLengthBytes;

		private IntPtr m_Data;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String RoomName
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

		public void Set(ref SendDataOptions other)
		{
		}

		public void Set(ref SendDataOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
