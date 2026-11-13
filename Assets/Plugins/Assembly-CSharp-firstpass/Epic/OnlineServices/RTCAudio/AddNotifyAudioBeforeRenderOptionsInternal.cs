using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct AddNotifyAudioBeforeRenderOptionsInternal : ISettable<AddNotifyAudioBeforeRenderOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private int m_UnmixedAudio;

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

		public bool UnmixedAudio
		{
			set
			{
			}
		}

		public void Set(ref AddNotifyAudioBeforeRenderOptions other)
		{
		}

		public void Set(ref AddNotifyAudioBeforeRenderOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
