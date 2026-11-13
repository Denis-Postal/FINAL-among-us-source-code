using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct OnQueryOutputDevicesInformationCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnQueryOutputDevicesInformationCallbackInfo>, ISettable<OnQueryOutputDevicesInformationCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

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

		public void Set(ref OnQueryOutputDevicesInformationCallbackInfo other)
		{
		}

		public void Set(ref OnQueryOutputDevicesInformationCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnQueryOutputDevicesInformationCallbackInfo output)
		{
			output = default;
		}
	}
}
