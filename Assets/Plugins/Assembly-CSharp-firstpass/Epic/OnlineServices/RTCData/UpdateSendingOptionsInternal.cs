using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct UpdateSendingOptionsInternal : ISettable<UpdateSendingOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private int m_DataEnabled;

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

		public bool DataEnabled
		{
			set
			{
			}
		}

		public void Set(ref UpdateSendingOptions other)
		{
		}

		public void Set(ref UpdateSendingOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
