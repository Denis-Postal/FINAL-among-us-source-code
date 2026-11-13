using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct AddNotifyRTCRoomConnectionChangedOptionsInternal : ISettable<AddNotifyRTCRoomConnectionChangedOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LobbyId_DEPRECATED;

		private IntPtr m_LocalUserId_DEPRECATED;

		public Utf8String LobbyId_DEPRECATED
		{
			set
			{
			}
		}

		public ProductUserId LocalUserId_DEPRECATED
		{
			set
			{
			}
		}

		public void Set(ref AddNotifyRTCRoomConnectionChangedOptions other)
		{
		}

		public void Set(ref AddNotifyRTCRoomConnectionChangedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
