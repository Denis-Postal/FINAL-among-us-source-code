using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct GetFriendsCountOptionsInternal : ISettable<GetFriendsCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref GetFriendsCountOptions other)
		{
		}

		public void Set(ref GetFriendsCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
