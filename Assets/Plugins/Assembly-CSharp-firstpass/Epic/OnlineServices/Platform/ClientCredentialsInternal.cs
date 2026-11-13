using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Platform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct ClientCredentialsInternal : IGettable<ClientCredentials>, ISettable<ClientCredentials>, IDisposable
	{
		private IntPtr m_ClientId;

		private IntPtr m_ClientSecret;

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

		public Utf8String ClientSecret
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref ClientCredentials other)
		{
		}

		public void Set(ref ClientCredentials? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out ClientCredentials output)
		{
			output = default;
		}
	}
}
