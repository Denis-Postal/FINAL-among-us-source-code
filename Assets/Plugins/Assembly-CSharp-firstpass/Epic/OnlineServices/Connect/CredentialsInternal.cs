using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CredentialsInternal : IGettable<Credentials>, ISettable<Credentials>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Token;

		private ExternalCredentialType m_Type;

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

		public ExternalCredentialType Type
		{
			get
			{
				return ExternalCredentialType.Epic;
			}
			set
			{
			}
		}

		public void Set(ref Credentials other)
		{
		}

		public void Set(ref Credentials? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out Credentials output)
		{
			output = default;
		}
	}
}
