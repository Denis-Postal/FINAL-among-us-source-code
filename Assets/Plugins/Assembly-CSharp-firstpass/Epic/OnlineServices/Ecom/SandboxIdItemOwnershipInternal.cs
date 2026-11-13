using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct SandboxIdItemOwnershipInternal : IGettable<SandboxIdItemOwnership>, ISettable<SandboxIdItemOwnership>, IDisposable
	{
		private IntPtr m_SandboxId;

		private IntPtr m_OwnedCatalogItemIds;

		private uint m_OwnedCatalogItemIdsCount;

		public Utf8String SandboxId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String[] OwnedCatalogItemIds
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref SandboxIdItemOwnership other)
		{
		}

		public void Set(ref SandboxIdItemOwnership? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SandboxIdItemOwnership output)
		{
			output = default;
		}
	}
}
