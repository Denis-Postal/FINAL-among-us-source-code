using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct InventoryItemSetValueAsBoolOptionsInternal : ISettable<InventoryItemSetValueAsBoolOptions>, IDisposable
	{
		private int m_ApiVersion;

		private int m_Value;

		public bool Value
		{
			set
			{
			}
		}

		public void Set(ref InventoryItemSetValueAsBoolOptions other)
		{
		}

		public void Set(ref InventoryItemSetValueAsBoolOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
