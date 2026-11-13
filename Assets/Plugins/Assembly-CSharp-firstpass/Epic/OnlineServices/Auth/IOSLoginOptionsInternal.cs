using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct IOSLoginOptionsInternal : ISettable<IOSLoginOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Credentials;

		private AuthScopeFlags m_ScopeFlags;

		private LoginFlags m_LoginFlags;

		public IOSCredentials? Credentials
		{
			set
			{
			}
		}

		public AuthScopeFlags ScopeFlags
		{
			set
			{
			}
		}

		public LoginFlags LoginFlags
		{
			set
			{
			}
		}

		public void Set(ref IOSLoginOptions other)
		{
		}

		public void Set(ref IOSLoginOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
