using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct OnCreateCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnCreateCompleteCallbackInfo>, ISettable<OnCreateCompleteCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_InventoryItemHandle;

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

		public InventoryItem InventoryItemHandle
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref OnCreateCompleteCallbackInfo other)
		{
		}

		public void Set(ref OnCreateCompleteCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnCreateCompleteCallbackInfo output)
		{
			output = default;
		}
	}
}
