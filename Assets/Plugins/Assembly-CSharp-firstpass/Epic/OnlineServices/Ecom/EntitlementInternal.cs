using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct EntitlementInternal : IGettable<Entitlement>, ISettable<Entitlement>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_EntitlementName;

		private IntPtr m_EntitlementId;

		private IntPtr m_CatalogItemId;

		private int m_ServerIndex;

		private int m_Redeemed;

		private long m_EndTimestamp;

		public Utf8String EntitlementName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String EntitlementId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String CatalogItemId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public int ServerIndex
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public bool Redeemed
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public long EndTimestamp
		{
			get
			{
				return 0L;
			}
			set
			{
			}
		}

		public void Set(ref Entitlement other)
		{
		}

		public void Set(ref Entitlement? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out Entitlement output)
		{
			output = default;
		}
	}
}
