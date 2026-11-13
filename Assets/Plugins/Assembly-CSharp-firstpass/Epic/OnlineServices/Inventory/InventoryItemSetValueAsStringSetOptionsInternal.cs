using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct InventoryItemSetValueAsStringSetOptionsInternal : ISettable<InventoryItemSetValueAsStringSetOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Value;

		private uint m_ValueCount;

		public Utf8String[] Value
		{
			set
			{
			}
		}

		public void Set(ref InventoryItemSetValueAsStringSetOptions other)
		{
		}

		public void Set(ref InventoryItemSetValueAsStringSetOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
