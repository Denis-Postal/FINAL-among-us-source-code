using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct HideFriendsOptionsInternal : ISettable<HideFriendsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref HideFriendsOptions other)
		{
		}

		public void Set(ref HideFriendsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
