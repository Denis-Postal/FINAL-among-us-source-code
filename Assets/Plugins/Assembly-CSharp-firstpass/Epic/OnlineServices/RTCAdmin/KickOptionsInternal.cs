using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct KickOptionsInternal : ISettable<KickOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_RoomName;

		private IntPtr m_TargetUserId;

		public Utf8String RoomName
		{
			set
			{
			}
		}

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public void Set(ref KickOptions other)
		{
		}

		public void Set(ref KickOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
