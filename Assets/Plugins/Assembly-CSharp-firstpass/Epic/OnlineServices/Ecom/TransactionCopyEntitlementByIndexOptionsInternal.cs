using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct TransactionCopyEntitlementByIndexOptionsInternal : ISettable<TransactionCopyEntitlementByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_EntitlementIndex;

		public uint EntitlementIndex
		{
			set
			{
			}
		}

		public void Set(ref TransactionCopyEntitlementByIndexOptions other)
		{
		}

		public void Set(ref TransactionCopyEntitlementByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
