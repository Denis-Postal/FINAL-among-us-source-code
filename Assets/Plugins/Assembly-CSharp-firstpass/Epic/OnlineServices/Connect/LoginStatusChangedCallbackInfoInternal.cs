using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LoginStatusChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LoginStatusChangedCallbackInfo>, ISettable<LoginStatusChangedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private LoginStatus m_PreviousStatus;

		private LoginStatus m_CurrentStatus;

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

		public ProductUserId LocalUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public LoginStatus PreviousStatus
		{
			get
			{
				return LoginStatus.NotLoggedIn;
			}
			set
			{
			}
		}

		public LoginStatus CurrentStatus
		{
			get
			{
				return LoginStatus.NotLoggedIn;
			}
			set
			{
			}
		}

		public void Set(ref LoginStatusChangedCallbackInfo other)
		{
		}

		public void Set(ref LoginStatusChangedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LoginStatusChangedCallbackInfo output)
		{
			output = default;
		}
	}
}
