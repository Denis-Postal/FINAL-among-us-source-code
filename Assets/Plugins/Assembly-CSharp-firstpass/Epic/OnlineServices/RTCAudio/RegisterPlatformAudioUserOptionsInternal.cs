using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct RegisterPlatformAudioUserOptionsInternal : ISettable<RegisterPlatformAudioUserOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_UserId;

		public Utf8String UserId
		{
			set
			{
			}
		}

		public void Set(ref RegisterPlatformAudioUserOptions other)
		{
		}

		public void Set(ref RegisterPlatformAudioUserOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
