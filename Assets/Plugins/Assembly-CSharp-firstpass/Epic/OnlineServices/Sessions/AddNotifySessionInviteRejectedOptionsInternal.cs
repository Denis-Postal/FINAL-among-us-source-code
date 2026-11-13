using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifySessionInviteRejectedOptionsInternal : ISettable<AddNotifySessionInviteRejectedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifySessionInviteRejectedOptions other)
		{
		}

		public void Set(ref AddNotifySessionInviteRejectedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
