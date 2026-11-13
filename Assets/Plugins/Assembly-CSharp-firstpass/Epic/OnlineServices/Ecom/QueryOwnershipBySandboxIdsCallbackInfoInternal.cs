using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct QueryOwnershipBySandboxIdsCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryOwnershipBySandboxIdsCallbackInfo>, ISettable<QueryOwnershipBySandboxIdsCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_SandboxIdItemOwnerships;

		private uint m_SandboxIdItemOwnershipsCount;

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

		public SandboxIdItemOwnership[] SandboxIdItemOwnerships
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref QueryOwnershipBySandboxIdsCallbackInfo other)
		{
		}

		public void Set(ref QueryOwnershipBySandboxIdsCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out QueryOwnershipBySandboxIdsCallbackInfo output)
		{
			output = default;
		}
	}
}
