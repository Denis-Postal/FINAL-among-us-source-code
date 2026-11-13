using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct UserLoginInfoInternal : IGettable<UserLoginInfo>, ISettable<UserLoginInfo>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_DisplayName;

		private IntPtr m_NsaIdToken;

		public Utf8String DisplayName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

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

		public void Set(ref UserLoginInfo other)
		{
		}

		public void Set(ref UserLoginInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out UserLoginInfo output)
		{
			output = default;
		}
	}
}
