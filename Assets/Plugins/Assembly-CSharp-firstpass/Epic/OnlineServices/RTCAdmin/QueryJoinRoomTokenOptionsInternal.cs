using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct QueryJoinRoomTokenOptionsInternal : ISettable<QueryJoinRoomTokenOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private IntPtr m_TargetUserIds;

		private uint m_TargetUserIdsCount;

		private IntPtr m_TargetUserIpAddresses;

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

		public ProductUserId[] TargetUserIds
		{
			set
			{
			}
		}

		public Utf8String TargetUserIpAddresses
		{
			set
			{
			}
		}

		public void Set(ref QueryJoinRoomTokenOptions other)
		{
		}

		public void Set(ref QueryJoinRoomTokenOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
