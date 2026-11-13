using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.IntegratedPlatform
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct UserPreLogoutCallbackInfoInternal : ICallbackInfoInternal, IGettable<UserPreLogoutCallbackInfo>, ISettable<UserPreLogoutCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_PlatformType;

		private IntPtr m_LocalPlatformUserId;

		private IntPtr m_AccountId;

		private IntPtr m_ProductUserId;

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

		public void Set(ref UserPreLogoutCallbackInfo other)
		{
		}

		public void Set(ref UserPreLogoutCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out UserPreLogoutCallbackInfo output)
		{
			output = default;
		}
	}
}
