using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifySessionInviteAcceptedOptionsInternal : ISettable<AddNotifySessionInviteAcceptedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifySessionInviteAcceptedOptions other)
		{
		}

		public void Set(ref AddNotifySessionInviteAcceptedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
