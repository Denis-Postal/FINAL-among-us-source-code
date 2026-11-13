using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct GetLocalPlatformTypeOptionsInternal : ISettable<GetLocalPlatformTypeOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref GetLocalPlatformTypeOptions other)
		{
		}

		public void Set(ref GetLocalPlatformTypeOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
