using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct GetAudioInputDevicesCountOptionsInternal : ISettable<GetAudioInputDevicesCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref GetAudioInputDevicesCountOptions other)
		{
		}

		public void Set(ref GetAudioInputDevicesCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
