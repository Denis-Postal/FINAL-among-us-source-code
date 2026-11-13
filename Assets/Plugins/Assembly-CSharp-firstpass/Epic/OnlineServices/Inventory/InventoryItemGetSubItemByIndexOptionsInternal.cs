using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct InventoryItemGetSubItemByIndexOptionsInternal : ISettable<InventoryItemGetSubItemByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_SubItemIndex;

		public uint SubItemIndex
		{
			set
			{
			}
		}

		public void Set(ref InventoryItemGetSubItemByIndexOptions other)
		{
		}

		public void Set(ref InventoryItemGetSubItemByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
