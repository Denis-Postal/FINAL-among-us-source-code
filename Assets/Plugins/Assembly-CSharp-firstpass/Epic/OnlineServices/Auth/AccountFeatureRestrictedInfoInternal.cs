using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct AccountFeatureRestrictedInfoInternal : IGettable<AccountFeatureRestrictedInfo>, ISettable<AccountFeatureRestrictedInfo>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_VerificationURI;

		public Utf8String VerificationURI
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref AccountFeatureRestrictedInfo other)
		{
		}

		public void Set(ref AccountFeatureRestrictedInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out AccountFeatureRestrictedInfo output)
		{
			output = default;
		}
	}
}
