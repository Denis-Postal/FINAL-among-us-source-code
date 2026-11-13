using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct AddNotifyAudioDevicesChangedOptionsInternal : ISettable<AddNotifyAudioDevicesChangedOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref AddNotifyAudioDevicesChangedOptions other)
		{
		}

		public void Set(ref AddNotifyAudioDevicesChangedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
