using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct InventoryItemGetValueTypeOptionsInternal : ISettable<InventoryItemGetValueTypeOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref InventoryItemGetValueTypeOptions other)
		{
		}

		public void Set(ref InventoryItemGetValueTypeOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
