using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct BestDisplayNameInternal : IGettable<BestDisplayName>, ISettable<BestDisplayName>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_UserId;

		private IntPtr m_DisplayName;

		private IntPtr m_DisplayNameSanitized;

		private IntPtr m_Nickname;

		private uint m_PlatformType;

		public EpicAccountId UserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

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

		public Utf8String DisplayNameSanitized
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String Nickname
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public uint PlatformType
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public void Set(ref BestDisplayName other)
		{
		}

		public void Set(ref BestDisplayName? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out BestDisplayName output)
		{
			output = default;
		}
	}
}
