using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct UpdateReceivingOptionsInternal : ISettable<UpdateReceivingOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_RoomName;

		private IntPtr m_ParticipantId;

		private int m_AudioEnabled;

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

		public ProductUserId ParticipantId
		{
			set
			{
			}
		}

		public bool AudioEnabled
		{
			set
			{
			}
		}

		public void Set(ref UpdateReceivingOptions other)
		{
		}

		public void Set(ref UpdateReceivingOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
