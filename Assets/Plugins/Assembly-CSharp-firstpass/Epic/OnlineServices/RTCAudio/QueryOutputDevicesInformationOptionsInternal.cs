using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct QueryOutputDevicesInformationOptionsInternal : ISettable<QueryOutputDevicesInformationOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref QueryOutputDevicesInformationOptions other)
		{
		}

		public void Set(ref QueryOutputDevicesInformationOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
