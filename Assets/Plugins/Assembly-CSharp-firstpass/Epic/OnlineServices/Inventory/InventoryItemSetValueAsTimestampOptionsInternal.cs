using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct InventoryItemSetValueAsTimestampOptionsInternal : ISettable<InventoryItemSetValueAsTimestampOptions>, IDisposable
	{
		private int m_ApiVersion;

		private long m_Value;

		public long Value
		{
			set
			{
			}
		}

		public void Set(ref InventoryItemSetValueAsTimestampOptions other)
		{
		}

		public void Set(ref InventoryItemSetValueAsTimestampOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
