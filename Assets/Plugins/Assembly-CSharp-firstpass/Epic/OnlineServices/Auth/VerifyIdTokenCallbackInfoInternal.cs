using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 96)]
	internal struct VerifyIdTokenCallbackInfoInternal : ICallbackInfoInternal, IGettable<VerifyIdTokenCallbackInfo>, ISettable<VerifyIdTokenCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_ApplicationId;

		private IntPtr m_ClientId;

		private IntPtr m_ProductId;

		private IntPtr m_SandboxId;

		private IntPtr m_DeploymentId;

		private IntPtr m_DisplayName;

		private int m_IsExternalAccountInfoPresent;

		private ExternalAccountType m_ExternalAccountIdType;

		private IntPtr m_ExternalAccountId;

		private IntPtr m_ExternalAccountDisplayName;

		private IntPtr m_Platform;

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

		public Utf8String ApplicationId
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

		public bool IsExternalAccountInfoPresent
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public ExternalAccountType ExternalAccountIdType
		{
			get
			{
				return ExternalAccountType.Epic;
			}
			set
			{
			}
		}

		public Utf8String ExternalAccountId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String ExternalAccountDisplayName
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
