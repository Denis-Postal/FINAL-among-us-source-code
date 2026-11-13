using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct QueryFileListOptionsInternal : ISettable<QueryFileListOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_ListOfTags;

		private uint m_ListOfTagsCount;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String[] ListOfTags
		{
			set
			{
			}
		}

		public void Set(ref QueryFileListOptions other)
		{
		}

		public void Set(ref QueryFileListOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
