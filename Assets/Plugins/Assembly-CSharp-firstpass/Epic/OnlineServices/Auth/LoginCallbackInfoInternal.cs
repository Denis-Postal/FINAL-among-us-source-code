using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 56)]
	internal struct LoginCallbackInfoInternal : ICallbackInfoInternal, IGettable<LoginCallbackInfo>, ISettable<LoginCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_PinGrantInfo;

		private IntPtr m_ContinuanceToken;

		private IntPtr m_AccountFeatureRestrictedInfo_DEPRECATED;

		private IntPtr m_SelectedAccountId;

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

		public EpicAccountId LocalUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public PinGrantInfo? PinGrantInfo
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ContinuanceToken ContinuanceToken
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public AccountFeatureRestrictedInfo? AccountFeatureRestrictedInfo_DEPRECATED
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public EpicAccountId SelectedAccountId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref LoginCallbackInfo other)
		{
		}

		public void Set(ref LoginCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LoginCallbackInfo output)
		{
			output = default;
		}
	}
}
