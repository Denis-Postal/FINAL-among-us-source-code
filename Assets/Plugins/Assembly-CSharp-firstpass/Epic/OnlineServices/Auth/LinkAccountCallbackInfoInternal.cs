using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct LinkAccountCallbackInfoInternal : ICallbackInfoInternal, IGettable<LinkAccountCallbackInfo>, ISettable<LinkAccountCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_PinGrantInfo;

		private IntPtr m_SelectedAccountId;

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

		public PinGrantInfo? PinGrantInfo
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public EpicAccountId SelectedAccountId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref LinkAccountCallbackInfo other)
		{
		}

		public void Set(ref LinkAccountCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LinkAccountCallbackInfo output)
		{
			output = default;
		}
	}
}
