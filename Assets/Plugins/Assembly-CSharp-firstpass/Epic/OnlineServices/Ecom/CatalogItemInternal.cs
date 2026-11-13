using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 88)]
	internal struct CatalogItemInternal : IGettable<CatalogItem>, ISettable<CatalogItem>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_CatalogNamespace;

		private IntPtr m_Id;

		private IntPtr m_EntitlementName;

		private IntPtr m_TitleText;

		private IntPtr m_DescriptionText;

		private IntPtr m_LongDescriptionText;

		private IntPtr m_TechnicalDetailsText;

		private IntPtr m_DeveloperText;

		private EcomItemType m_ItemType;

		private long m_EntitlementEndTimestamp;

		public Utf8String CatalogNamespace
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

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

		public Utf8String TitleText
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String DescriptionText
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String LongDescriptionText
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String TechnicalDetailsText
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String DeveloperText
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public EcomItemType ItemType
		{
			get
			{
				return EcomItemType.Durable;
			}
			set
			{
			}
		}

		public long EntitlementEndTimestamp
		{
			get
			{
				return 0L;
			}
			set
			{
			}
		}

		public void Set(ref CatalogItem other)
		{
		}

		public void Set(ref CatalogItem? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out CatalogItem output)
		{
			output = default;
		}
	}
}
