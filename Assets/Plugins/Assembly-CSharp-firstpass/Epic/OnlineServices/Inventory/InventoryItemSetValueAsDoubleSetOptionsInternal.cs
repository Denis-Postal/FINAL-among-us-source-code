using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct InventoryItemSetValueAsDoubleSetOptionsInternal : ISettable<InventoryItemSetValueAsDoubleSetOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Value;

		private uint m_ValueCount;

		public double[] Value
		{
			set
			{
			}
		}

		public void Set(ref InventoryItemSetValueAsDoubleSetOptions other)
		{
		}

		public void Set(ref InventoryItemSetValueAsDoubleSetOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
