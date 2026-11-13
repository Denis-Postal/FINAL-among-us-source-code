using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct CheckoutEntryInternal : IGettable<CheckoutEntry>, ISettable<CheckoutEntry>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_OfferId;

		public Utf8String OfferId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref CheckoutEntry other)
		{
		}

		public void Set(ref CheckoutEntry? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out CheckoutEntry output)
		{
			output = default;
		}
	}
}
