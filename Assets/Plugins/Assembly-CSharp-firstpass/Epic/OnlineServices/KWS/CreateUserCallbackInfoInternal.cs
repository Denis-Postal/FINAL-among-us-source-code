using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct CreateUserCallbackInfoInternal : ICallbackInfoInternal, IGettable<CreateUserCallbackInfo>, ISettable<CreateUserCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_KWSUserId;

		private int m_IsMinor;

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

		public Utf8String KWSUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool IsMinor
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public void Set(ref CreateUserCallbackInfo other)
		{
		}

		public void Set(ref CreateUserCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out CreateUserCallbackInfo output)
		{
			output = default;
		}
	}
}
