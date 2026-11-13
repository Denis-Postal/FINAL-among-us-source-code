using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct InventoryItemGetValueAsTimestampOptionsInternal : ISettable<InventoryItemGetValueAsTimestampOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref InventoryItemGetValueAsTimestampOptions other)
		{
		}

		public void Set(ref InventoryItemGetValueAsTimestampOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
