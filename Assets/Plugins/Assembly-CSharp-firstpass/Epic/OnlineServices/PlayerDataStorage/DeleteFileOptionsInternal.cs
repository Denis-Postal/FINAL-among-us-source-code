using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct DeleteFileOptionsInternal : ISettable<DeleteFileOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_Filename;

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

		public void Set(ref DeleteFileOptions other)
		{
		}

		public void Set(ref DeleteFileOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
