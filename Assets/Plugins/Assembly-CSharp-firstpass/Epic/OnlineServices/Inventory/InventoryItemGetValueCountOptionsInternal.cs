using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct InventoryItemGetValueCountOptionsInternal : ISettable<InventoryItemGetValueCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref InventoryItemGetValueCountOptions other)
		{
		}

		public void Set(ref InventoryItemGetValueCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
