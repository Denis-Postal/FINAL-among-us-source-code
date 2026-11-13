using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct InventoryItemGetValueAsBoolOptionsInternal : ISettable<InventoryItemGetValueAsBoolOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref InventoryItemGetValueAsBoolOptions other)
		{
		}

		public void Set(ref InventoryItemGetValueAsBoolOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
