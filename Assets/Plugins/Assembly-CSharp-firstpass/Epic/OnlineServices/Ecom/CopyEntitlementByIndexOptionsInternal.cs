using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyEntitlementByIndexOptionsInternal : ISettable<CopyEntitlementByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private uint m_EntitlementIndex;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public uint EntitlementIndex
		{
			set
			{
			}
		}

		public void Set(ref CopyEntitlementByIndexOptions other)
		{
		}

		public void Set(ref CopyEntitlementByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
