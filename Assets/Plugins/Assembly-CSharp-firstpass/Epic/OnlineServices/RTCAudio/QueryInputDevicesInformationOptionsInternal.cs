using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct QueryInputDevicesInformationOptionsInternal : ISettable<QueryInputDevicesInformationOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref QueryInputDevicesInformationOptions other)
		{
		}

		public void Set(ref QueryInputDevicesInformationOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
