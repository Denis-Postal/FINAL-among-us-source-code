using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct InventoryItemSetValueAsInt64SetOptionsInternal : ISettable<InventoryItemSetValueAsInt64SetOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Value;

		private uint m_ValueCount;

		public long[] Value
		{
			set
			{
			}
		}

		public void Set(ref InventoryItemSetValueAsInt64SetOptions other)
		{
		}

		public void Set(ref InventoryItemSetValueAsInt64SetOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
