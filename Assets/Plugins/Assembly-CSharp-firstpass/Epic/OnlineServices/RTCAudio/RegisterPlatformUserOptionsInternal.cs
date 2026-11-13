using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct RegisterPlatformUserOptionsInternal : ISettable<RegisterPlatformUserOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PlatformUserId;

		public Utf8String PlatformUserId
		{
			set
			{
			}
		}

		public void Set(ref RegisterPlatformUserOptions other)
		{
		}

		public void Set(ref RegisterPlatformUserOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
