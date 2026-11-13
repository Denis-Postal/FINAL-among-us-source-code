using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct RejectInviteOptionsInternal : ISettable<RejectInviteOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_InviteId;

		private IntPtr m_LocalUserId;

		public Utf8String InviteId
		{
			set
			{
			}
		}

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref RejectInviteOptions other)
		{
		}

		public void Set(ref RejectInviteOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
