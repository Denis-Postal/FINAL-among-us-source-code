using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct GetEntitlementsByNameCountOptionsInternal : ISettable<GetEntitlementsByNameCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_EntitlementName;

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

		public void Set(ref GetEntitlementsByNameCountOptions other)
		{
		}

		public void Set(ref GetEntitlementsByNameCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
