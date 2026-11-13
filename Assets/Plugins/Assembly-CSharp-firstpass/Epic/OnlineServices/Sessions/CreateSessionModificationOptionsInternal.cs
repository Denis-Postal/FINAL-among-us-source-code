using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 80)]
	internal struct CreateSessionModificationOptionsInternal : ISettable<CreateSessionModificationOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_SessionName;

		private IntPtr m_BucketId;

		private uint m_MaxPlayers;

		private IntPtr m_LocalUserId;

		private int m_PresenceEnabled;

		private IntPtr m_SessionId;

		private int m_SanctionsEnabled;

		private IntPtr m_AllowedPlatformIds;

		private uint m_AllowedPlatformIdsCount;

		public Utf8String SessionName
		{
			set
			{
			}
		}

		public Utf8String BucketId
		{
			set
			{
			}
		}

		public uint MaxPlayers
		{
			set
			{
			}
		}

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public bool PresenceEnabled
		{
			set
			{
			}
		}

		public Utf8String SessionId
		{
			set
			{
			}
		}

		public bool SanctionsEnabled
		{
			set
			{
			}
		}

		public uint[] AllowedPlatformIds
		{
			set
			{
			}
		}

		public void Set(ref CreateSessionModificationOptions other)
		{
		}

		public void Set(ref CreateSessionModificationOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
