using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ReceiptValidator
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct PS5IAPVerifyPurchaseOptionsPlatformSpecificOptionsInternal : IGettable<PS5IAPVerifyPurchaseOptionsPlatformSpecificOptions>, ISettable<PS5IAPVerifyPurchaseOptionsPlatformSpecificOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_EntitlementLabel;

		private IntPtr m_ServiceLabel;

		private IntPtr m_IdToken;

		private IntPtr m_AccessToken;

		public Utf8String EntitlementLabel
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String ServiceLabel
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String IdToken
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String AccessToken
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref PS5IAPVerifyPurchaseOptionsPlatformSpecificOptions other)
		{
		}

		public void Set(ref PS5IAPVerifyPurchaseOptionsPlatformSpecificOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PS5IAPVerifyPurchaseOptionsPlatformSpecificOptions output)
		{
			output = default;
		}
	}
}
