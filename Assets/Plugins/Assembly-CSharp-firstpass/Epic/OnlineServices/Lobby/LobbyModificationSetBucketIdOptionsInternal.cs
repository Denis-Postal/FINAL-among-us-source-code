using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct LobbyModificationSetBucketIdOptionsInternal : ISettable<LobbyModificationSetBucketIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_BucketId;

		public Utf8String BucketId
		{
			set
			{
			}
		}

		public void Set(ref LobbyModificationSetBucketIdOptions other)
		{
		}

		public void Set(ref LobbyModificationSetBucketIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
