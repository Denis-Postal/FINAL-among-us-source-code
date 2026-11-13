using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifySendSessionNativeInviteRequestedOptionsInternal : ISettable<AddNotifySendSessionNativeInviteRequestedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifySendSessionNativeInviteRequestedOptions other)
		{
		}

		public void Set(ref AddNotifySendSessionNativeInviteRequestedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
