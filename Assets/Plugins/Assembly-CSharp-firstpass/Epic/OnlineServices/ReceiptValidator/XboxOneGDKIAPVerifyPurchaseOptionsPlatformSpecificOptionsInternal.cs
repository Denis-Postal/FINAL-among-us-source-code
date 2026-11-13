using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ReceiptValidator
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct XboxOneGDKIAPVerifyPurchaseOptionsPlatformSpecificOptionsInternal : IGettable<XboxOneGDKIAPVerifyPurchaseOptionsPlatformSpecificOptions>, ISettable<XboxOneGDKIAPVerifyPurchaseOptionsPlatformSpecificOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ItemId;

		private IntPtr m_XSTSToken;

		public Utf8String ItemId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String XSTSToken
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref XboxOneGDKIAPVerifyPurchaseOptionsPlatformSpecificOptions other)
		{
		}

		public void Set(ref XboxOneGDKIAPVerifyPurchaseOptionsPlatformSpecificOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out XboxOneGDKIAPVerifyPurchaseOptionsPlatformSpecificOptions output)
		{
			output = default;
		}
	}
}
