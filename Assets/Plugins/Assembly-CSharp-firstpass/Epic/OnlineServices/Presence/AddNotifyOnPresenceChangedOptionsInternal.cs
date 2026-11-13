using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyOnPresenceChangedOptionsInternal : ISettable<AddNotifyOnPresenceChangedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyOnPresenceChangedOptions other)
		{
		}

		public void Set(ref AddNotifyOnPresenceChangedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
