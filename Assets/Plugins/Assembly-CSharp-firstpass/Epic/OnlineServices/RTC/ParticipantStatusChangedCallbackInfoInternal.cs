using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 56)]
	internal struct ParticipantStatusChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<ParticipantStatusChangedCallbackInfo>, ISettable<ParticipantStatusChangedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private IntPtr m_ParticipantId;

		private RTCParticipantStatus m_ParticipantStatus;

		private uint m_ParticipantMetadataCount;

		private IntPtr m_ParticipantMetadata;

		private int m_ParticipantInBlocklist;

		public object ClientData
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public IntPtr ClientDataAddress => (IntPtr)0;

		public ProductUserId LocalUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String RoomName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ProductUserId ParticipantId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public RTCParticipantStatus ParticipantStatus
		{
			get
			{
				return RTCParticipantStatus.Joined;
			}
			set
			{
			}
		}

		public ParticipantMetadata[] ParticipantMetadata
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool ParticipantInBlocklist
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public void Set(ref ParticipantStatusChangedCallbackInfo other)
		{
		}

		public void Set(ref ParticipantStatusChangedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out ParticipantStatusChangedCallbackInfo output)
		{
			output = default;
		}
	}
}
