using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct QueryFileListCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryFileListCallbackInfo>, ISettable<QueryFileListCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private uint m_FileCount;

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

		public uint FileCount
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public void Set(ref QueryFileListCallbackInfo other)
		{
		}

		public void Set(ref QueryFileListCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out QueryFileListCallbackInfo output)
		{
			output = default;
		}
	}
}
