using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct AudioDevicesChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<AudioDevicesChangedCallbackInfo>, ISettable<AudioDevicesChangedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

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

		public void Set(ref AudioDevicesChangedCallbackInfo other)
		{
		}

		public void Set(ref AudioDevicesChangedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out AudioDevicesChangedCallbackInfo output)
		{
			output = default;
		}
	}
}
