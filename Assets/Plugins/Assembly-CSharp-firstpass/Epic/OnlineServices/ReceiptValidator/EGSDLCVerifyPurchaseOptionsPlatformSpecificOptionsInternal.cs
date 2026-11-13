using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ReceiptValidator
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct EGSDLCVerifyPurchaseOptionsPlatformSpecificOptionsInternal : IGettable<EGSDLCVerifyPurchaseOptionsPlatformSpecificOptions>, ISettable<EGSDLCVerifyPurchaseOptionsPlatformSpecificOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_CatalogItemId;

		private IntPtr m_EntitlementId;

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

		public void Set(ref EGSDLCVerifyPurchaseOptionsPlatformSpecificOptions other)
		{
		}

		public void Set(ref EGSDLCVerifyPurchaseOptionsPlatformSpecificOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out EGSDLCVerifyPurchaseOptionsPlatformSpecificOptions output)
		{
			output = default;
		}
	}
}
