using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct UnregisterPlatformUserOptionsInternal : ISettable<UnregisterPlatformUserOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PlatformUserId;

		public Utf8String PlatformUserId
		{
			set
			{
			}
		}

		public void Set(ref UnregisterPlatformUserOptions other)
		{
		}

		public void Set(ref UnregisterPlatformUserOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
