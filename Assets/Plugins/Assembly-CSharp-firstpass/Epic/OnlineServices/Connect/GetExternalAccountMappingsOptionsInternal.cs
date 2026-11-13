using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct GetExternalAccountMappingsOptionsInternal : ISettable<GetExternalAccountMappingsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private ExternalAccountType m_AccountIdType;

		private IntPtr m_TargetExternalUserId;

		public ProductUserId LocalUserId
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

		public Utf8String TargetExternalUserId
		{
			set
			{
			}
		}

		public void Set(ref GetExternalAccountMappingsOptions other)
		{
		}

		public void Set(ref GetExternalAccountMappingsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
