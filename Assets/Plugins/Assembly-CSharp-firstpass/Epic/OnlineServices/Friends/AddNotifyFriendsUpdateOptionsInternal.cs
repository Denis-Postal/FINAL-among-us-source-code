using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyFriendsUpdateOptionsInternal : ISettable<AddNotifyFriendsUpdateOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyFriendsUpdateOptions other)
		{
		}

		public void Set(ref AddNotifyFriendsUpdateOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
