using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 88)]
	internal struct VerifyIdTokenCallbackInfoInternal : ICallbackInfoInternal, IGettable<VerifyIdTokenCallbackInfo>, ISettable<VerifyIdTokenCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_ProductUserId;

		private int m_IsAccountInfoPresent;

		private ExternalAccountType m_AccountIdType;

		private IntPtr m_AccountId;

		private IntPtr m_Platform;

		private IntPtr m_DeviceType;

		private IntPtr m_ClientId;

		private IntPtr m_ProductId;

		private IntPtr m_SandboxId;

		private IntPtr m_DeploymentId;

		public Result ResultCode
		{
			get
			{
				return Result.Success;
			}
			set
			{
			}
		}

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

		public bool IsAccountInfoPresent
		{
			get
			{
				return false;
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

		public Utf8String Platform
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String DeviceType
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String ClientId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String ProductId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String SandboxId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String DeploymentId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref VerifyIdTokenCallbackInfo other)
		{
		}

		public void Set(ref VerifyIdTokenCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out VerifyIdTokenCallbackInfo output)
		{
			output = default;
		}
	}
}
