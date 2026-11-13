using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct AddNotifyAudioInputStateOptionsInternal : ISettable<AddNotifyAudioInputStateOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

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

		public void Set(ref AddNotifyAudioInputStateOptions other)
		{
		}

		public void Set(ref AddNotifyAudioInputStateOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
