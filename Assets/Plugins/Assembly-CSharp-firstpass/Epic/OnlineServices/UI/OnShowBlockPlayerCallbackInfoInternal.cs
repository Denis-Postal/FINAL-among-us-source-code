using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct OnShowBlockPlayerCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnShowBlockPlayerCallbackInfo>, ISettable<OnShowBlockPlayerCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

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

		public EpicAccountId TargetUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref OnShowBlockPlayerCallbackInfo other)
		{
		}

		public void Set(ref OnShowBlockPlayerCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnShowBlockPlayerCallbackInfo output)
		{
			output = default;
		}
	}
}
