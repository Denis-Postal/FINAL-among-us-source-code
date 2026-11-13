using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct GetAudioInputDeviceByIndexOptionsInternal : ISettable<GetAudioInputDeviceByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_DeviceInfoIndex;

		public uint DeviceInfoIndex
		{
			set
			{
			}
		}

		public void Set(ref GetAudioInputDeviceByIndexOptions other)
		{
		}

		public void Set(ref GetAudioInputDeviceByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
