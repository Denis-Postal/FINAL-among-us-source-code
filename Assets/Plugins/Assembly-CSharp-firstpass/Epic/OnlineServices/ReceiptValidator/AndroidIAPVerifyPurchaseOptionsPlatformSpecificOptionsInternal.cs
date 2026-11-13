using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ReceiptValidator
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct AndroidIAPVerifyPurchaseOptionsPlatformSpecificOptionsInternal : IGettable<AndroidIAPVerifyPurchaseOptionsPlatformSpecificOptions>, ISettable<AndroidIAPVerifyPurchaseOptionsPlatformSpecificOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ProductId;

		private IntPtr m_Token;

		public Utf8String ProductId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String Token
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref AndroidIAPVerifyPurchaseOptionsPlatformSpecificOptions other)
		{
		}

		public void Set(ref AndroidIAPVerifyPurchaseOptionsPlatformSpecificOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out AndroidIAPVerifyPurchaseOptionsPlatformSpecificOptions output)
		{
			output = default;
		}
	}
}
