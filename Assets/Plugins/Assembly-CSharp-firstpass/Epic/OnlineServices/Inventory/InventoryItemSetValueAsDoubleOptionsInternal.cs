using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct InventoryItemSetValueAsDoubleOptionsInternal : ISettable<InventoryItemSetValueAsDoubleOptions>, IDisposable
	{
		private int m_ApiVersion;

		private double m_Value;

		public double Value
		{
			set
			{
			}
		}

		public void Set(ref InventoryItemSetValueAsDoubleOptions other)
		{
		}

		public void Set(ref InventoryItemSetValueAsDoubleOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
