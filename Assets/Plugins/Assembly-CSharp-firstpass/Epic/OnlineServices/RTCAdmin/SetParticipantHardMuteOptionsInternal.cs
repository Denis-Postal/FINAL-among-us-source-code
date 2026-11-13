using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct SetParticipantHardMuteOptionsInternal : ISettable<SetParticipantHardMuteOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_RoomName;

		private IntPtr m_TargetUserId;

		private int m_Mute;

		public Utf8String RoomName
		{
			set
			{
			}
		}

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public bool Mute
		{
			set
			{
			}
		}

		public void Set(ref SetParticipantHardMuteOptions other)
		{
		}

		public void Set(ref SetParticipantHardMuteOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
