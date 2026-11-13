using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyProductUserExternalAccountByAccountIdOptionsInternal : ISettable<CopyProductUserExternalAccountByAccountIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_TargetUserId;

		private IntPtr m_AccountId;

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public Utf8String AccountId
		{
			set
			{
			}
		}

		public void Set(ref CopyProductUserExternalAccountByAccountIdOptions other)
		{
		}

		public void Set(ref CopyProductUserExternalAccountByAccountIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
