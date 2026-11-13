using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct UpdateSendingCallbackInfoInternal : ICallbackInfoInternal, IGettable<UpdateSendingCallbackInfo>, ISettable<UpdateSendingCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private RTCAudioStatus m_AudioStatus;

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

		public void Set(ref UpdateSendingCallbackInfo other)
		{
		}

		public void Set(ref UpdateSendingCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out UpdateSendingCallbackInfo output)
		{
			output = default;
		}
	}
}
