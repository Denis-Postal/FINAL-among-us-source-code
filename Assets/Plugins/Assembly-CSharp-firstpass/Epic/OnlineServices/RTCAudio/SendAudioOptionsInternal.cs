using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct SendAudioOptionsInternal : ISettable<SendAudioOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private IntPtr m_Buffer;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String RoomName
		{
			set
			{
			}
		}

		public AudioBuffer? Buffer
		{
			set
			{
			}
		}

		public void Set(ref SendAudioOptions other)
		{
		}

		public void Set(ref SendAudioOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
