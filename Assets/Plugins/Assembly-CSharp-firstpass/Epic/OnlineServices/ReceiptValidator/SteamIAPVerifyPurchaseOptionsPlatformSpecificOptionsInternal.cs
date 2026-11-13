using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ReceiptValidator
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct SteamIAPVerifyPurchaseOptionsPlatformSpecificOptionsInternal : IGettable<SteamIAPVerifyPurchaseOptionsPlatformSpecificOptions>, ISettable<SteamIAPVerifyPurchaseOptionsPlatformSpecificOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_TransId;

		public Utf8String TransId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref SteamIAPVerifyPurchaseOptionsPlatformSpecificOptions other)
		{
		}

		public void Set(ref SteamIAPVerifyPurchaseOptionsPlatformSpecificOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SteamIAPVerifyPurchaseOptionsPlatformSpecificOptions output)
		{
			output = default;
		}
	}
}
