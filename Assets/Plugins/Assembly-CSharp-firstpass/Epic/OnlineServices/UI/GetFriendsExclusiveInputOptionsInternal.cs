using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct GetFriendsExclusiveInputOptionsInternal : ISettable<GetFriendsExclusiveInputOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref GetFriendsExclusiveInputOptions other)
		{
		}

		public void Set(ref GetFriendsExclusiveInputOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
