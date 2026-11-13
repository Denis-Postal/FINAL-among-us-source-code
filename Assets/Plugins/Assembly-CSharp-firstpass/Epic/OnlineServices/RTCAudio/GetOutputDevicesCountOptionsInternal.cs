using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct GetOutputDevicesCountOptionsInternal : ISettable<GetOutputDevicesCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref GetOutputDevicesCountOptions other)
		{
		}

		public void Set(ref GetOutputDevicesCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
