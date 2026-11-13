using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct ExternalUserInfoInternal : IGettable<ExternalUserInfo>, ISettable<ExternalUserInfo>, IDisposable
	{
		private int m_ApiVersion;

		private ExternalAccountType m_AccountType;

		private IntPtr m_AccountId;

		private IntPtr m_DisplayName;

		private IntPtr m_DisplayNameSanitized;

		public ExternalAccountType AccountType
		{
			get
			{
				return ExternalAccountType.Epic;
			}
			set
			{
			}
		}

		public Utf8String AccountId
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

		public void Set(ref ExternalUserInfo other)
		{
		}

		public void Set(ref ExternalUserInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out ExternalUserInfo output)
		{
			output = default;
		}
	}
}
