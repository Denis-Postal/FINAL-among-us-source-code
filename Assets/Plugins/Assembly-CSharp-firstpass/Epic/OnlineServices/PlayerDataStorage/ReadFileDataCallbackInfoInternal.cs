using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct ReadFileDataCallbackInfoInternal : ICallbackInfoInternal, IGettable<ReadFileDataCallbackInfo>, ISettable<ReadFileDataCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_Filename;

		private uint m_TotalFileSizeBytes;

		private int m_IsLastChunk;

		private uint m_DataChunkLengthBytes;

		private IntPtr m_DataChunk;

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

		public Utf8String Filename
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public uint TotalFileSizeBytes
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public bool IsLastChunk
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public ArraySegment<byte> DataChunk
		{
			get
			{
				return default;
			}
			set
			{
			}
		}

		public void Set(ref ReadFileDataCallbackInfo other)
		{
		}

		public void Set(ref ReadFileDataCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out ReadFileDataCallbackInfo output)
		{
			output = default;
		}
	}
}
