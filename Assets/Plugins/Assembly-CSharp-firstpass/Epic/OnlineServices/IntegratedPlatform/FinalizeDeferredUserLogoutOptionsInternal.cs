using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct FinalizeDeferredUserLogoutOptionsInternal : ISettable<FinalizeDeferredUserLogoutOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PlatformType;

		private IntPtr m_LocalPlatformUserId;

		private LoginStatus m_ExpectedLoginStatus;

		public Utf8String PlatformType
		{
			set
			{
			}
		}

		public Utf8String LocalPlatformUserId
		{
			set
			{
			}
		}

		public LoginStatus ExpectedLoginStatus
		{
			set
			{
			}
		}

		public void Set(ref FinalizeDeferredUserLogoutOptions other)
		{
		}

		public void Set(ref FinalizeDeferredUserLogoutOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
