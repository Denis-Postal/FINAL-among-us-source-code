using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyCustomInviteAcceptedOptionsInternal : ISettable<AddNotifyCustomInviteAcceptedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyCustomInviteAcceptedOptions other)
		{
		}

		public void Set(ref AddNotifyCustomInviteAcceptedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
