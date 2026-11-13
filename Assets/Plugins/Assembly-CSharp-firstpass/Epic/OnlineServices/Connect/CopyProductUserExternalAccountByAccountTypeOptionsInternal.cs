using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyProductUserExternalAccountByAccountTypeOptionsInternal : ISettable<CopyProductUserExternalAccountByAccountTypeOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_TargetUserId;

		private ExternalAccountType m_AccountIdType;

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public ExternalAccountType AccountIdType
		{
			set
			{
			}
		}

		public void Set(ref CopyProductUserExternalAccountByAccountTypeOptions other)
		{
		}

		public void Set(ref CopyProductUserExternalAccountByAccountTypeOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
