using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct PresenceModificationSetJoinInfoOptionsInternal : ISettable<PresenceModificationSetJoinInfoOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_JoinInfo;

		public Utf8String JoinInfo
		{
			set
			{
			}
		}

		public void Set(ref PresenceModificationSetJoinInfoOptions other)
		{
		}

		public void Set(ref PresenceModificationSetJoinInfoOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
