using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct OnRegisterPlatformUserCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnRegisterPlatformUserCallbackInfo>, ISettable<OnRegisterPlatformUserCallbackInfo>, IDisposable
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

		public void Set(ref OnRegisterPlatformUserCallbackInfo other)
		{
		}

		public void Set(ref OnRegisterPlatformUserCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnRegisterPlatformUserCallbackInfo output)
		{
			output = default;
		}
	}
}
