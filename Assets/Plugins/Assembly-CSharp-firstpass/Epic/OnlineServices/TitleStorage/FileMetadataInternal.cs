using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct FileMetadataInternal : IGettable<FileMetadata>, ISettable<FileMetadata>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_FileSizeBytes;

		private IntPtr m_MD5Hash;

		private IntPtr m_Filename;

		private uint m_UnencryptedDataSizeBytes;

		public uint FileSizeBytes
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public Utf8String MD5Hash
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

		public uint UnencryptedDataSizeBytes
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public void Set(ref FileMetadata other)
		{
		}

		public void Set(ref FileMetadata? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out FileMetadata output)
		{
			output = default;
		}
	}
}
