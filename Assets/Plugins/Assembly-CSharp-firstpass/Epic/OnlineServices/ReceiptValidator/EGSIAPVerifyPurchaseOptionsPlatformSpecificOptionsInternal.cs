using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ReceiptValidator
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct EGSIAPVerifyPurchaseOptionsPlatformSpecificOptionsInternal : IGettable<EGSIAPVerifyPurchaseOptionsPlatformSpecificOptions>, ISettable<EGSIAPVerifyPurchaseOptionsPlatformSpecificOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_EntitlementName;

		private IntPtr m_EntitlementId;

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

		public void Set(ref EGSIAPVerifyPurchaseOptionsPlatformSpecificOptions other)
		{
		}

		public void Set(ref EGSIAPVerifyPurchaseOptionsPlatformSpecificOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out EGSIAPVerifyPurchaseOptionsPlatformSpecificOptions output)
		{
			output = default;
		}
	}
}
