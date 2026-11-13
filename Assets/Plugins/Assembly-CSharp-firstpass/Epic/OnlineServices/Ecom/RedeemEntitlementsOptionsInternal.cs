using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct RedeemEntitlementsOptionsInternal : ISettable<RedeemEntitlementsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private uint m_EntitlementIdCount;

		private IntPtr m_EntitlementIds;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String[] EntitlementIds
		{
			set
			{
			}
		}

		public void Set(ref RedeemEntitlementsOptions other)
		{
		}

		public void Set(ref RedeemEntitlementsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
