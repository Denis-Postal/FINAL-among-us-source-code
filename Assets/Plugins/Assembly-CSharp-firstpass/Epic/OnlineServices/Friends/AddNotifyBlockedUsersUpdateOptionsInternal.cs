using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyBlockedUsersUpdateOptionsInternal : ISettable<AddNotifyBlockedUsersUpdateOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyBlockedUsersUpdateOptions other)
		{
		}

		public void Set(ref AddNotifyBlockedUsersUpdateOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
