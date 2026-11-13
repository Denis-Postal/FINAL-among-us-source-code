using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct SetPresenceOptionsInternal : ISettable<SetPresenceOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_PresenceModificationHandle;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public PresenceModification PresenceModificationHandle
		{
			set
			{
			}
		}

		public void Set(ref SetPresenceOptions other)
		{
		}

		public void Set(ref SetPresenceOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
