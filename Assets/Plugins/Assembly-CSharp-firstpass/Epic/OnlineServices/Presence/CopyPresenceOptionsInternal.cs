using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyPresenceOptionsInternal : ISettable<CopyPresenceOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public EpicAccountId TargetUserId
		{
			set
			{
			}
		}

		public void Set(ref CopyPresenceOptions other)
		{
		}

		public void Set(ref CopyPresenceOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
