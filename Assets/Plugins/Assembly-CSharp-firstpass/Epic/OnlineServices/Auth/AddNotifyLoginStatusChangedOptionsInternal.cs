using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyLoginStatusChangedOptionsInternal : ISettable<AddNotifyLoginStatusChangedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyLoginStatusChangedOptions other)
		{
		}

		public void Set(ref AddNotifyLoginStatusChangedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
