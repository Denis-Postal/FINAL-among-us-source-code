using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct InventoryItemSetValueAsStringOptionsInternal : ISettable<InventoryItemSetValueAsStringOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Value;

		public Utf8String Value
		{
			set
			{
			}
		}

		public void Set(ref InventoryItemSetValueAsStringOptions other)
		{
		}

		public void Set(ref InventoryItemSetValueAsStringOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
