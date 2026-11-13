using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct QueryEntitlementTokenOptionsInternal : ISettable<QueryEntitlementTokenOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_EntitlementNames;

		private uint m_EntitlementNameCount;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String[] EntitlementNames
		{
			set
			{
			}
		}

		public void Set(ref QueryEntitlementTokenOptions other)
		{
		}

		public void Set(ref QueryEntitlementTokenOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
