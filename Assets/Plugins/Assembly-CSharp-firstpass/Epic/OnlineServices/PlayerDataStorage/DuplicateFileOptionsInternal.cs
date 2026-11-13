using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct DuplicateFileOptionsInternal : ISettable<DuplicateFileOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_SourceFilename;

		private IntPtr m_DestinationFilename;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String SourceFilename
		{
			set
			{
			}
		}

		public Utf8String DestinationFilename
		{
			set
			{
			}
		}

		public void Set(ref DuplicateFileOptions other)
		{
		}

		public void Set(ref DuplicateFileOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
