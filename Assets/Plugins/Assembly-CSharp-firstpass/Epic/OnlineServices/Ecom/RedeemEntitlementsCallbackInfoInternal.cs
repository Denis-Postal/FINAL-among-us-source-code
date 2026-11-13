using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct RedeemEntitlementsCallbackInfoInternal : ICallbackInfoInternal, IGettable<RedeemEntitlementsCallbackInfo>, ISettable<RedeemEntitlementsCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private uint m_RedeemedEntitlementIdsCount;

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

		public uint RedeemedEntitlementIdsCount
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public void Set(ref RedeemEntitlementsCallbackInfo other)
		{
		}

		public void Set(ref RedeemEntitlementsCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out RedeemEntitlementsCallbackInfo output)
		{
			output = default;
		}
	}
}
