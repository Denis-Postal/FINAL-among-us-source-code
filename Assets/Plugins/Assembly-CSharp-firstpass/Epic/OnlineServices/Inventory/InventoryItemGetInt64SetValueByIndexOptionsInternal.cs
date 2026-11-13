using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct InventoryItemGetInt64SetValueByIndexOptionsInternal : ISettable<InventoryItemGetInt64SetValueByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_SetValueIndex;

		public uint SetValueIndex
		{
			set
			{
			}
		}

		public void Set(ref InventoryItemGetInt64SetValueByIndexOptions other)
		{
		}

		public void Set(ref InventoryItemGetInt64SetValueByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
