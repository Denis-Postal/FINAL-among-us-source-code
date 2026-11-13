using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct ExternalAccountInfoInternal : IGettable<ExternalAccountInfo>, ISettable<ExternalAccountInfo>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ProductUserId;

		private IntPtr m_DisplayName;

		private IntPtr m_AccountId;

		private ExternalAccountType m_AccountIdType;

		private long m_LastLoginTime;

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

		public ExternalAccountType AccountIdType
		{
			get
			{
				return ExternalAccountType.Epic;
			}
			set
			{
			}
		}

		public DateTimeOffset? LastLoginTime
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref ExternalAccountInfo other)
		{
		}

		public void Set(ref ExternalAccountInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out ExternalAccountInfo output)
		{
			output = default;
		}
	}
}
