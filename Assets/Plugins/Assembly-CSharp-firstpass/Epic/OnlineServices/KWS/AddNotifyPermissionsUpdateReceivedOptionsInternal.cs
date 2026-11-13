using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyPermissionsUpdateReceivedOptionsInternal : ISettable<AddNotifyPermissionsUpdateReceivedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyPermissionsUpdateReceivedOptions other)
		{
		}

		public void Set(ref AddNotifyPermissionsUpdateReceivedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
