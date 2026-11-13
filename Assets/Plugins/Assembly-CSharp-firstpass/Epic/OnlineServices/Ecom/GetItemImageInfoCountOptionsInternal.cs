using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct GetItemImageInfoCountOptionsInternal : ISettable<GetItemImageInfoCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_ItemId;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String ItemId
		{
			set
			{
			}
		}

		public void Set(ref GetItemImageInfoCountOptions other)
		{
		}

		public void Set(ref GetItemImageInfoCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
