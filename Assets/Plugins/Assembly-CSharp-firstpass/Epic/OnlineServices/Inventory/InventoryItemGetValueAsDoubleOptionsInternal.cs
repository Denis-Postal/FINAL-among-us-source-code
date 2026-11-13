using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct InventoryItemGetValueAsDoubleOptionsInternal : ISettable<InventoryItemGetValueAsDoubleOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref InventoryItemGetValueAsDoubleOptions other)
		{
		}

		public void Set(ref InventoryItemGetValueAsDoubleOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
