using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct InventoryItemGetDoubleSetValueByIndexOptionsInternal : ISettable<InventoryItemGetDoubleSetValueByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_SetValueIndex;

		public uint SetValueIndex
		{
			set
			{
			}
		}

		public void Set(ref InventoryItemGetDoubleSetValueByIndexOptions other)
		{
		}

		public void Set(ref InventoryItemGetDoubleSetValueByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
