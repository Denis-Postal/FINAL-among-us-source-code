using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 64)]
	internal struct JoinRoomOptionsInternal : ISettable<JoinRoomOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private IntPtr m_ClientBaseUrl;

		private IntPtr m_ParticipantToken;

		private IntPtr m_ParticipantId;

		private JoinRoomFlags m_Flags;

		private int m_ManualAudioInputEnabled;

		private int m_ManualAudioOutputEnabled;

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

		public Utf8String ClientBaseUrl
		{
			set
			{
			}
		}

		public Utf8String ParticipantToken
		{
			set
			{
			}
		}

		public ProductUserId ParticipantId
		{
			set
			{
			}
		}

		public JoinRoomFlags Flags
		{
			set
			{
			}
		}

		public bool ManualAudioInputEnabled
		{
			set
			{
			}
		}

		public bool ManualAudioOutputEnabled
		{
			set
			{
			}
		}

		public void Set(ref JoinRoomOptions other)
		{
		}

		public void Set(ref JoinRoomOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
