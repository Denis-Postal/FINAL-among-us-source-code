using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct InventoryItemGetValueAsInt64OptionsInternal : ISettable<InventoryItemGetValueAsInt64Options>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref InventoryItemGetValueAsInt64Options other)
		{
		}

		public void Set(ref InventoryItemGetValueAsInt64Options? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
