using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct InventoryItemSetKeyOptionsInternal : ISettable<InventoryItemSetKeyOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Key;

		public Utf8String Key
		{
			set
			{
			}
		}

		public void Set(ref InventoryItemSetKeyOptions other)
		{
		}

		public void Set(ref InventoryItemSetKeyOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
