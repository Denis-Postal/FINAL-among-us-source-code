using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ReceiptValidator
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct SteamIAPVerifyPurchaseOptionsInternal : ISettable<SteamIAPVerifyPurchaseOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

		private ReceiptValidatorVendorType m_VendorType;

		private ReceiptValidatorEntitlementType m_EntitlementType;

		private IntPtr m_PlatformSpecificOptions;

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

		public ReceiptValidatorVendorType VendorType
		{
			set
			{
			}
		}

		public ReceiptValidatorEntitlementType EntitlementType
		{
			set
			{
			}
		}

		public SteamIAPVerifyPurchaseOptionsPlatformSpecificOptions? PlatformSpecificOptions
		{
			set
			{
			}
		}

		public void Set(ref SteamIAPVerifyPurchaseOptions other)
		{
		}

		public void Set(ref SteamIAPVerifyPurchaseOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
