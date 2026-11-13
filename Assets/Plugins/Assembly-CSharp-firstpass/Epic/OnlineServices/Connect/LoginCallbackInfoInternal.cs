using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct LoginCallbackInfoInternal : ICallbackInfoInternal, IGettable<LoginCallbackInfo>, ISettable<LoginCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_ContinuanceToken;

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
