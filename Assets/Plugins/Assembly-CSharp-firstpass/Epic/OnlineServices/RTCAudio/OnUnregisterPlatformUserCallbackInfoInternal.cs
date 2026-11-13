using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct OnUnregisterPlatformUserCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnUnregisterPlatformUserCallbackInfo>, ISettable<OnUnregisterPlatformUserCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_PlatformUserId;

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

		public Utf8String PlatformUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref OnUnregisterPlatformUserCallbackInfo other)
		{
		}

		public void Set(ref OnUnregisterPlatformUserCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnUnregisterPlatformUserCallbackInfo output)
		{
			output = default;
		}
	}
}
