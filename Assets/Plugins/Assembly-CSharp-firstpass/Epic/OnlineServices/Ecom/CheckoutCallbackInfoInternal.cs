using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct CheckoutCallbackInfoInternal : ICallbackInfoInternal, IGettable<CheckoutCallbackInfo>, ISettable<CheckoutCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_TransactionId;

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

		public Utf8String TransactionId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref CheckoutCallbackInfo other)
		{
		}

		public void Set(ref CheckoutCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out CheckoutCallbackInfo output)
		{
			output = default;
		}
	}
}
