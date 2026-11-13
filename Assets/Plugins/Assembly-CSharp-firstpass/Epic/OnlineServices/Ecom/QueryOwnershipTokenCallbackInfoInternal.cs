using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct QueryOwnershipTokenCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryOwnershipTokenCallbackInfo>, ISettable<QueryOwnershipTokenCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_OwnershipToken;

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

		public Utf8String OwnershipToken
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref QueryOwnershipTokenCallbackInfo other)
		{
		}

		public void Set(ref QueryOwnershipTokenCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out QueryOwnershipTokenCallbackInfo output)
		{
			output = default;
		}
	}
}
