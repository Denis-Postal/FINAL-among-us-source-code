using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct CopyEntitlementByNameAndIndexOptionsInternal : ISettable<CopyEntitlementByNameAndIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_EntitlementName;

		private uint m_Index;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String EntitlementName
		{
			set
			{
			}
		}

		public uint Index
		{
			set
			{
			}
		}

		public void Set(ref CopyEntitlementByNameAndIndexOptions other)
		{
		}

		public void Set(ref CopyEntitlementByNameAndIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
