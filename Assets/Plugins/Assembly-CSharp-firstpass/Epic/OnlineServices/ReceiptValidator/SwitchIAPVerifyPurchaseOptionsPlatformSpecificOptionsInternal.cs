using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ReceiptValidator
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct SwitchIAPVerifyPurchaseOptionsPlatformSpecificOptionsInternal : IGettable<SwitchIAPVerifyPurchaseOptionsPlatformSpecificOptions>, ISettable<SwitchIAPVerifyPurchaseOptionsPlatformSpecificOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_NsaIdToken;

		public Utf8String NsaIdToken
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref SwitchIAPVerifyPurchaseOptionsPlatformSpecificOptions other)
		{
		}

		public void Set(ref SwitchIAPVerifyPurchaseOptionsPlatformSpecificOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SwitchIAPVerifyPurchaseOptionsPlatformSpecificOptions output)
		{
			output = default;
		}
	}
}
