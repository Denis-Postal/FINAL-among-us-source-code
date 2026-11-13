using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct InventoryItemGetValueAsStringOptionsInternal : ISettable<InventoryItemGetValueAsStringOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref InventoryItemGetValueAsStringOptions other)
		{
		}

		public void Set(ref InventoryItemGetValueAsStringOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
