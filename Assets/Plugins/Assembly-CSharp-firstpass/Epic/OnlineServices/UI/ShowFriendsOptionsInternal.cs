using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct ShowFriendsOptionsInternal : ISettable<ShowFriendsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref ShowFriendsOptions other)
		{
		}

		public void Set(ref ShowFriendsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
