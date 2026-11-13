using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyLastRedeemedEntitlementByIndexOptionsInternal : ISettable<CopyLastRedeemedEntitlementByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private uint m_RedeemedEntitlementIndex;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public uint RedeemedEntitlementIndex
		{
			set
			{
			}
		}

		public void Set(ref CopyLastRedeemedEntitlementByIndexOptions other)
		{
		}

		public void Set(ref CopyLastRedeemedEntitlementByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
