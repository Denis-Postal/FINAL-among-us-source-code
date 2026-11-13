using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ReceiptValidator
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct VerifyPurchaseCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<VerifyPurchaseCompleteCallbackInfo>, ISettable<VerifyPurchaseCompleteCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

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

		public void Set(ref VerifyPurchaseCompleteCallbackInfo other)
		{
		}

		public void Set(ref VerifyPurchaseCompleteCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out VerifyPurchaseCompleteCallbackInfo output)
		{
			output = default;
		}
	}
}
