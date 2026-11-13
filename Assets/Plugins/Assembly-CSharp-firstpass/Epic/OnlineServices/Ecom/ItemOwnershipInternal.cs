using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct ItemOwnershipInternal : IGettable<ItemOwnership>, ISettable<ItemOwnership>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Id;

		private OwnershipStatus m_OwnershipStatus;

		public Utf8String Id
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public OwnershipStatus OwnershipStatus
		{
			get
			{
				return OwnershipStatus.NotOwned;
			}
			set
			{
			}
		}

		public void Set(ref ItemOwnership other)
		{
		}

		public void Set(ref ItemOwnership? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out ItemOwnership output)
		{
			output = default;
		}
	}
}
