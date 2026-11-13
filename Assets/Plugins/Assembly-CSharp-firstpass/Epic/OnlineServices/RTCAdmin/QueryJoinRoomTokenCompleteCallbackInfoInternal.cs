using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct QueryJoinRoomTokenCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryJoinRoomTokenCompleteCallbackInfo>, ISettable<QueryJoinRoomTokenCompleteCallbackInfo>, IDisposable
	{
		private Result m_ResultCode;

		private IntPtr m_ClientData;

		private IntPtr m_RoomName;

		private IntPtr m_ClientBaseUrl;

		private uint m_QueryId;

		private uint m_TokenCount;

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

		public Utf8String ClientBaseUrl
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public uint QueryId
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public uint TokenCount
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public void Set(ref QueryJoinRoomTokenCompleteCallbackInfo other)
		{
		}

		public void Set(ref QueryJoinRoomTokenCompleteCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out QueryJoinRoomTokenCompleteCallbackInfo output)
		{
			output = default;
		}
	}
}
