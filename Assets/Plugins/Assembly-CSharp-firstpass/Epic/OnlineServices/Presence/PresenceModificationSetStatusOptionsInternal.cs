using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct PresenceModificationSetStatusOptionsInternal : ISettable<PresenceModificationSetStatusOptions>, IDisposable
	{
		private int m_ApiVersion;

		private Status m_Status;

		public Status Status
		{
			set
			{
			}
		}

		public void Set(ref PresenceModificationSetStatusOptions other)
		{
		}

		public void Set(ref PresenceModificationSetStatusOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
