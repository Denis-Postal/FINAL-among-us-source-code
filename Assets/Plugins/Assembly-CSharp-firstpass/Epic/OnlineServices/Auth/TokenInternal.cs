using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 88)]
	internal struct TokenInternal : IGettable<Token>, ISettable<Token>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_App;

		private IntPtr m_ClientId;

		private IntPtr m_AccountId;

		private IntPtr m_AccessToken;

		private double m_ExpiresIn;

		private IntPtr m_ExpiresAt;

		private AuthTokenType m_AuthType;

		private IntPtr m_RefreshToken;

		private double m_RefreshExpiresIn;

		private IntPtr m_RefreshExpiresAt;

		public Utf8String App
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String ClientId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public EpicAccountId AccountId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String AccessToken
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public double ExpiresIn
		{
			get
			{
				return 0.0;
			}
			set
			{
			}
		}

		public Utf8String ExpiresAt
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public AuthTokenType AuthType
		{
			get
			{
				return AuthTokenType.Client;
			}
			set
			{
			}
		}

		public Utf8String RefreshToken
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public double RefreshExpiresIn
		{
			get
			{
				return 0.0;
			}
			set
			{
			}
		}

		public Utf8String RefreshExpiresAt
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref Token other)
		{
		}

		public void Set(ref Token? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out Token output)
		{
			output = default;
		}
	}
}
