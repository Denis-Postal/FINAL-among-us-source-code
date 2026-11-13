using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct RejectInviteOptionsInternal : ISettable<RejectInviteOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_InviteId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String InviteId
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
