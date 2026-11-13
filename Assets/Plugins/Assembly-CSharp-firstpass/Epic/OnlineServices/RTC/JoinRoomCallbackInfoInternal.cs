using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct JoinRoomCallbackInfoInternal : ICallbackInfoInternal, IGettable<JoinRoomCallbackInfo>, ISettable<JoinRoomCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private uint m_RoomOptionsCount;

		private IntPtr m_RoomOptions;

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

		public Option[] RoomOptions
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref JoinRoomCallbackInfo other)
		{
		}

		public void Set(ref JoinRoomCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out JoinRoomCallbackInfo output)
		{
			output = default;
		}
	}
}
