using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct SetDisplayPreferenceOptionsInternal : ISettable<SetDisplayPreferenceOptions>, IDisposable
	{
		private int m_ApiVersion;

		private NotificationLocation m_NotificationLocation;

		public NotificationLocation NotificationLocation
		{
			set
			{
			}
		}

		public void Set(ref SetDisplayPreferenceOptions other)
		{
		}

		public void Set(ref SetDisplayPreferenceOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
