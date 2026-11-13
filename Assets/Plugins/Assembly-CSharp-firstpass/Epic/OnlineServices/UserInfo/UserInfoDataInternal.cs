using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 56)]
	internal struct UserInfoDataInternal : IGettable<UserInfoData>, ISettable<UserInfoData>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_UserId;

		private IntPtr m_Country;

		private IntPtr m_DisplayName;

		private IntPtr m_PreferredLanguage;

		private IntPtr m_Nickname;

		private IntPtr m_DisplayNameSanitized;

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

		public Utf8String Country
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

		public Utf8String PreferredLanguage
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

		public void Set(ref UserInfoData other)
		{
		}

		public void Set(ref UserInfoData? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out UserInfoData output)
		{
			output = default;
		}
	}
}
