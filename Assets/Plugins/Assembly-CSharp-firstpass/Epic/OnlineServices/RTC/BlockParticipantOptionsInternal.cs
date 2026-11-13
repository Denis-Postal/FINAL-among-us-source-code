using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct BlockParticipantOptionsInternal : ISettable<BlockParticipantOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private IntPtr m_ParticipantId;

		private int m_Blocked;

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

		public ProductUserId ParticipantId
		{
			set
			{
			}
		}

		public bool Blocked
		{
			set
			{
			}
		}

		public void Set(ref BlockParticipantOptions other)
		{
		}

		public void Set(ref BlockParticipantOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
