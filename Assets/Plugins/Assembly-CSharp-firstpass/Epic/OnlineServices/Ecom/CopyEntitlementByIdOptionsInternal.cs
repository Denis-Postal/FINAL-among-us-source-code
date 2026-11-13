using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyEntitlementByIdOptionsInternal : ISettable<CopyEntitlementByIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_EntitlementId;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String EntitlementId
		{
			set
			{
			}
		}

		public void Set(ref CopyEntitlementByIdOptions other)
		{
		}

		public void Set(ref CopyEntitlementByIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
