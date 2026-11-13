using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyRequestToJoinAcceptedOptionsInternal : ISettable<AddNotifyRequestToJoinAcceptedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyRequestToJoinAcceptedOptions other)
		{
		}

		public void Set(ref AddNotifyRequestToJoinAcceptedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
