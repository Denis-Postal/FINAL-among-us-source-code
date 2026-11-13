using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct UserTokenInternal : IGettable<UserToken>, ISettable<UserToken>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ProductUserId;

		private IntPtr m_Token;

		public ProductUserId ProductUserId
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

		public void Set(ref UserToken other)
		{
		}

		public void Set(ref UserToken? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out UserToken output)
		{
			output = default;
		}
	}
}
