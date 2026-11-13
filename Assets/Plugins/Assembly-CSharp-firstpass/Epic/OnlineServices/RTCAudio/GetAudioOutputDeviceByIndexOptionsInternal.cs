using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct GetAudioOutputDeviceByIndexOptionsInternal : ISettable<GetAudioOutputDeviceByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_DeviceInfoIndex;

		public uint DeviceInfoIndex
		{
			set
			{
			}
		}

		public void Set(ref GetAudioOutputDeviceByIndexOptions other)
		{
		}

		public void Set(ref GetAudioOutputDeviceByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
