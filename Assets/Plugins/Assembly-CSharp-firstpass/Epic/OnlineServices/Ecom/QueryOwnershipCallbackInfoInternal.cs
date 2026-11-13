using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct QueryOwnershipCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryOwnershipCallbackInfo>, ISettable<QueryOwnershipCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_ItemOwnership;

		private uint m_ItemOwnershipCount;

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

		public ItemOwnership[] ItemOwnership
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref QueryOwnershipCallbackInfo other)
		{
		}

		public void Set(ref QueryOwnershipCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out QueryOwnershipCallbackInfo output)
		{
			output = default;
		}
	}
}
