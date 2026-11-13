using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct LoginOptionsInternal : ISettable<LoginOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Credentials;

		private AuthScopeFlags m_ScopeFlags;

		private LoginFlags m_LoginFlags;

		public Credentials? Credentials
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

		public void Set(ref LoginOptions other)
		{
		}

		public void Set(ref LoginOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
