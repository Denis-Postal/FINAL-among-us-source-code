using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct IOSCredentialsInternal : IGettable<IOSCredentials>, ISettable<IOSCredentials>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Id;

		private IntPtr m_Token;

		private LoginCredentialType m_Type;

		private IntPtr m_SystemAuthCredentialsOptions;

		private ExternalCredentialType m_ExternalType;

		public Utf8String Id
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String Token
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public LoginCredentialType Type
		{
			get
			{
				return LoginCredentialType.Password;
			}
			set
			{
			}
		}

		public IOSCredentialsSystemAuthCredentialsOptions? SystemAuthCredentialsOptions
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ExternalCredentialType ExternalType
		{
			get
			{
				return ExternalCredentialType.Epic;
			}
			set
			{
			}
		}

		public void Set(ref IOSCredentials other)
		{
		}

		public void Set(ref IOSCredentials? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out IOSCredentials output)
		{
			output = default;
		}
	}
}
