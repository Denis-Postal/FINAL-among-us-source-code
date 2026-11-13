using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct UnregisterPlatformAudioUserOptionsInternal : ISettable<UnregisterPlatformAudioUserOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_UserId;

		public Utf8String UserId
		{
			set
			{
			}
		}

		public void Set(ref UnregisterPlatformAudioUserOptions other)
		{
		}

		public void Set(ref UnregisterPlatformAudioUserOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
