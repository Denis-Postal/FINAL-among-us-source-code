using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct ParticipantUpdatedCallbackInfoInternal : ICallbackInfoInternal, IGettable<ParticipantUpdatedCallbackInfo>, ISettable<ParticipantUpdatedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private IntPtr m_ParticipantId;

		private int m_Speaking;

		private RTCAudioStatus m_AudioStatus;

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

		public bool Speaking
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public RTCAudioStatus AudioStatus
		{
			get
			{
				return RTCAudioStatus.Unsupported;
			}
			set
			{
			}
		}

		public void Set(ref ParticipantUpdatedCallbackInfo other)
		{
		}

		public void Set(ref ParticipantUpdatedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out ParticipantUpdatedCallbackInfo output)
		{
			output = default;
		}
	}
}
