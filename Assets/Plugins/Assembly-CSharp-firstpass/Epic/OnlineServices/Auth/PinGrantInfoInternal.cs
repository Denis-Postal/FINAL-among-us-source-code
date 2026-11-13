using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct PinGrantInfoInternal : IGettable<PinGrantInfo>, ISettable<PinGrantInfo>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_UserCode;

		private IntPtr m_VerificationURI;

		private int m_ExpiresIn;

		private IntPtr m_VerificationURIComplete;

		public Utf8String UserCode
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String VerificationURI
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public int ExpiresIn
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public Utf8String VerificationURIComplete
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref PinGrantInfo other)
		{
		}

		public void Set(ref PinGrantInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PinGrantInfo output)
		{
			output = default;
		}
	}
}
