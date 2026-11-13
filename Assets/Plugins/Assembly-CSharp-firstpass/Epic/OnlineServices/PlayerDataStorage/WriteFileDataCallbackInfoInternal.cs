using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct WriteFileDataCallbackInfoInternal : ICallbackInfoInternal, IGettable<WriteFileDataCallbackInfo>, ISettable<WriteFileDataCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_Filename;

		private uint m_DataBufferLengthBytes;

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

		public uint DataBufferLengthBytes
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public void Set(ref WriteFileDataCallbackInfo other)
		{
		}

		public void Set(ref WriteFileDataCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out WriteFileDataCallbackInfo output)
		{
			output = default;
		}
	}
}
