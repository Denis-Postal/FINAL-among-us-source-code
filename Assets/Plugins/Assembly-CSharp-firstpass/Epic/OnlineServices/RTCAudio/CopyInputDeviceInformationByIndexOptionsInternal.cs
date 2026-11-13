using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct CopyInputDeviceInformationByIndexOptionsInternal : ISettable<CopyInputDeviceInformationByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_DeviceIndex;

		public uint DeviceIndex
		{
			set
			{
			}
		}

		public void Set(ref CopyInputDeviceInformationByIndexOptions other)
		{
		}

		public void Set(ref CopyInputDeviceInformationByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
