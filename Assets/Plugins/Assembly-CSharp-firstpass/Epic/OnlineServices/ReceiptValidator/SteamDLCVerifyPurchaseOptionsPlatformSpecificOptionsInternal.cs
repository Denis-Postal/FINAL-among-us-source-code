using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ReceiptValidator
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct SteamDLCVerifyPurchaseOptionsPlatformSpecificOptionsInternal : IGettable<SteamDLCVerifyPurchaseOptionsPlatformSpecificOptions>, ISettable<SteamDLCVerifyPurchaseOptionsPlatformSpecificOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_AppId;

		private IntPtr m_SteamId;

		public Utf8String AppId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String SteamId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref SteamDLCVerifyPurchaseOptionsPlatformSpecificOptions other)
		{
		}

		public void Set(ref SteamDLCVerifyPurchaseOptionsPlatformSpecificOptions? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SteamDLCVerifyPurchaseOptionsPlatformSpecificOptions output)
		{
			output = default;
		}
	}
}
