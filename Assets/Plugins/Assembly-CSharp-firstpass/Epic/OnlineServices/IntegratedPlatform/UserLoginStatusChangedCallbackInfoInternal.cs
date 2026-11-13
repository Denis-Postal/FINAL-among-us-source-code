using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct UserLoginStatusChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<UserLoginStatusChangedCallbackInfo>, ISettable<UserLoginStatusChangedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_PlatformType;

		private IntPtr m_LocalPlatformUserId;

		private IntPtr m_AccountId;

		private IntPtr m_ProductUserId;

		private LoginStatus m_PreviousLoginStatus;

		private LoginStatus m_CurrentLoginStatus;

		public object ClientData
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public IntPtr ClientDataAddress => (IntPtr)0;

		public Utf8String PlatformType
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String LocalPlatformUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public EpicAccountId AccountId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

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

		public LoginStatus PreviousLoginStatus
		{
			get
			{
				return LoginStatus.NotLoggedIn;
			}
			set
			{
			}
		}

		public LoginStatus CurrentLoginStatus
		{
			get
			{
				return LoginStatus.NotLoggedIn;
			}
			set
			{
			}
		}

		public void Set(ref UserLoginStatusChangedCallbackInfo other)
		{
		}

		public void Set(ref UserLoginStatusChangedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out UserLoginStatusChangedCallbackInfo output)
		{
			output = default;
		}
	}
}
