using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct CopyOutputDeviceInformationByIndexOptionsInternal : ISettable<CopyOutputDeviceInformationByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_DeviceIndex;

		public uint DeviceIndex
		{
			set
			{
			}
		}

		public void Set(ref CopyOutputDeviceInformationByIndexOptions other)
		{
		}

		public void Set(ref CopyOutputDeviceInformationByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
