using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct BlockParticipantCallbackInfoInternal : ICallbackInfoInternal, IGettable<BlockParticipantCallbackInfo>, ISettable<BlockParticipantCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private IntPtr m_ParticipantId;

		private int m_Blocked;

		public Result ResultCode
		{
			get
			{
				return Result.Success;
			}
			set
			{
			}
		}

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

		public bool Blocked
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public void Set(ref BlockParticipantCallbackInfo other)
		{
		}

		public void Set(ref BlockParticipantCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out BlockParticipantCallbackInfo output)
		{
			output = default;
		}
	}
}
