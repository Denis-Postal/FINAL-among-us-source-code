using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ReceiptValidator
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct IOSIAPVerifyPurchaseOptionsPlatformSpecificOptionsInternal : IGettable<IOSIAPVerifyPurchaseOptionsPlatformSpecificOptions>, ISettable<IOSIAPVerifyPurchaseOptionsPlatformSpecificOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_TransactionId;

		private IntPtr m_Receipt;

		public Utf8String TransactionId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String Receipt
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref IOSIAPVerifyPurchaseOptionsPlatformSpecificOptions other)
		{
		}

		public void Set(ref IOSIAPVerifyPurchaseOptionsPlatformSpecificOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out IOSIAPVerifyPurchaseOptionsPlatformSpecificOptions output)
		{
			output = default;
		}
	}
}
