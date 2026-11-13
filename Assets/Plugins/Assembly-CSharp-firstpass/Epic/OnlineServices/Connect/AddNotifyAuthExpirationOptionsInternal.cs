using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyAuthExpirationOptionsInternal : ISettable<AddNotifyAuthExpirationOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyAuthExpirationOptions other)
		{
		}

		public void Set(ref AddNotifyAuthExpirationOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
