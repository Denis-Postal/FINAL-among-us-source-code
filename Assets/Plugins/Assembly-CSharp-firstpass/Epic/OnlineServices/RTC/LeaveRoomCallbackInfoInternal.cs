using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct LeaveRoomCallbackInfoInternal : ICallbackInfoInternal, IGettable<LeaveRoomCallbackInfo>, ISettable<LeaveRoomCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

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

		public void Set(ref LeaveRoomCallbackInfo other)
		{
		}

		public void Set(ref LeaveRoomCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LeaveRoomCallbackInfo output)
		{
			output = default;
		}
	}
}
