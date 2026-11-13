using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct CloseOptionsInternal : ISettable<CloseOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

		private IntPtr m_InventoryName;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public Utf8String InventoryName
		{
			set
			{
			}
		}

		public void Set(ref CloseOptions other)
		{
		}

		public void Set(ref CloseOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
