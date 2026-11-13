using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct ReadFileOptionsInternal : ISettable<ReadFileOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_Filename;

		private uint m_ReadChunkLengthBytes;

		private IntPtr m_ReadFileDataCallback;

		private IntPtr m_FileTransferProgressCallback;

		private static OnReadFileDataCallbackInternal s_ReadFileDataCallback;

		private static OnFileTransferProgressCallbackInternal s_FileTransferProgressCallback;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String Filename
		{
			set
			{
			}
		}

		public uint ReadChunkLengthBytes
		{
			set
			{
			}
		}

		public static OnReadFileDataCallbackInternal ReadFileDataCallback => null;

		public static OnFileTransferProgressCallbackInternal FileTransferProgressCallback => null;

		public void Set(ref ReadFileOptions other)
		{
		}

		public void Set(ref ReadFileOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
