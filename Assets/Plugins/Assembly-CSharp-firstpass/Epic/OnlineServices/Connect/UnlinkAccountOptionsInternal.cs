using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct UnlinkAccountOptionsInternal : ISettable<UnlinkAccountOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref UnlinkAccountOptions other)
		{
		}

		public void Set(ref UnlinkAccountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
