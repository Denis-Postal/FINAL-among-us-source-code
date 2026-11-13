using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct RoomStatisticsUpdatedInfoInternal : ICallbackInfoInternal, IGettable<RoomStatisticsUpdatedInfo>, ISettable<RoomStatisticsUpdatedInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private IntPtr m_Statistic;

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

		public Utf8String Statistic
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref RoomStatisticsUpdatedInfo other)
		{
		}

		public void Set(ref RoomStatisticsUpdatedInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out RoomStatisticsUpdatedInfo output)
		{
			output = default;
		}
	}
}
