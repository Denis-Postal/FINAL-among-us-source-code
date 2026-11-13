using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyCustomInviteRejectedOptionsInternal : ISettable<AddNotifyCustomInviteRejectedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyCustomInviteRejectedOptions other)
		{
		}

		public void Set(ref AddNotifyCustomInviteRejectedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
