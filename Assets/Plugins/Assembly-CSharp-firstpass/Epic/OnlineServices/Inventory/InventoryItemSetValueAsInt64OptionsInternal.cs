using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct InventoryItemSetValueAsInt64OptionsInternal : ISettable<InventoryItemSetValueAsInt64Options>, IDisposable
	{
		private int m_ApiVersion;

		private long m_Value;

		public long Value
		{
			set
			{
			}
		}

		public void Set(ref InventoryItemSetValueAsInt64Options other)
		{
		}

		public void Set(ref InventoryItemSetValueAsInt64Options? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
