using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyRequestToJoinRejectedOptionsInternal : ISettable<AddNotifyRequestToJoinRejectedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyRequestToJoinRejectedOptions other)
		{
		}

		public void Set(ref AddNotifyRequestToJoinRejectedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
