using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct IdTokenInternal : IGettable<IdToken>, ISettable<IdToken>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_AccountId;

		private IntPtr m_JsonWebToken;

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

		public Utf8String JsonWebToken
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref IdToken other)
		{
		}

		public void Set(ref IdToken? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out IdToken output)
		{
			output = default;
		}
	}
}
