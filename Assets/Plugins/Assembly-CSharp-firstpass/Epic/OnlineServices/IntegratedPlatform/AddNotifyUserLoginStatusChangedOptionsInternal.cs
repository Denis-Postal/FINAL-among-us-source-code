using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyUserLoginStatusChangedOptionsInternal : ISettable<AddNotifyUserLoginStatusChangedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyUserLoginStatusChangedOptions other)
		{
		}

		public void Set(ref AddNotifyUserLoginStatusChangedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
