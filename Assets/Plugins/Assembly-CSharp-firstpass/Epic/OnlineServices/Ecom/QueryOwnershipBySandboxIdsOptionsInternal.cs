using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct QueryOwnershipBySandboxIdsOptionsInternal : ISettable<QueryOwnershipBySandboxIdsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_SandboxIds;

		private uint m_SandboxIdsCount;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String[] SandboxIds
		{
			set
			{
			}
		}

		public void Set(ref QueryOwnershipBySandboxIdsOptions other)
		{
		}

		public void Set(ref QueryOwnershipBySandboxIdsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
