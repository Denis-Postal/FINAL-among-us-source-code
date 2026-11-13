using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct FileTransferProgressCallbackInfoInternal : ICallbackInfoInternal, IGettable<FileTransferProgressCallbackInfo>, ISettable<FileTransferProgressCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_Filename;

		private uint m_BytesTransferred;

		private uint m_TotalFileSizeBytes;

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

		public uint BytesTransferred
		{
			get
			{
				return 0u;
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

		public void Set(ref FileTransferProgressCallbackInfo other)
		{
		}

		public void Set(ref FileTransferProgressCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out FileTransferProgressCallbackInfo output)
		{
			output = default;
		}
	}
}
