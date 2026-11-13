using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct OnSetInputDeviceSettingsCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnSetInputDeviceSettingsCallbackInfo>, ISettable<OnSetInputDeviceSettingsCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_RealDeviceId;

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

		public Utf8String RealDeviceId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref OnSetInputDeviceSettingsCallbackInfo other)
		{
		}

		public void Set(ref OnSetInputDeviceSettingsCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnSetInputDeviceSettingsCallbackInfo output)
		{
			output = default;
		}
	}
}
