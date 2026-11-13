using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct CopySessionHandleByInviteIdOptionsInternal : ISettable<CopySessionHandleByInviteIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_InviteId;

		public Utf8String InviteId
		{
			set
			{
			}
		}

		public void Set(ref CopySessionHandleByInviteIdOptions other)
		{
		}

		public void Set(ref CopySessionHandleByInviteIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
