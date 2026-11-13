using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatClient
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 56)]
	internal struct RegisterPeerOptionsInternal : ISettable<RegisterPeerOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PeerHandle;

		private AntiCheatCommonClientType m_ClientType;

		private AntiCheatCommonClientPlatform m_ClientPlatform;

		private uint m_AuthenticationTimeout;

		private IntPtr m_AccountId_DEPRECATED;

		private IntPtr m_IpAddress;

		private IntPtr m_PeerProductUserId;

		public IntPtr PeerHandle
		{
			set
			{
			}
		}

		public AntiCheatCommonClientType ClientType
		{
			set
			{
			}
		}

		public AntiCheatCommonClientPlatform ClientPlatform
		{
			set
			{
			}
		}

		public uint AuthenticationTimeout
		{
			set
			{
			}
		}

		public Utf8String AccountId_DEPRECATED
		{
			set
			{
			}
		}

		public Utf8String IpAddress
		{
			set
			{
			}
		}

		public ProductUserId PeerProductUserId
		{
			set
			{
			}
		}

		public void Set(ref RegisterPeerOptions other)
		{
		}

		public void Set(ref RegisterPeerOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
