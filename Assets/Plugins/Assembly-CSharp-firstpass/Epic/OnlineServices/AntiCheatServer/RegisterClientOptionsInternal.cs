using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.AntiCheatCommon;

namespace Epic.OnlineServices.AntiCheatServer
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct RegisterClientOptionsInternal : ISettable<RegisterClientOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ClientHandle;

		private AntiCheatCommonClientType m_ClientType;

		private AntiCheatCommonClientPlatform m_ClientPlatform;

		private IntPtr m_AccountId_DEPRECATED;

		private IntPtr m_IpAddress;

		private IntPtr m_UserId;

		public IntPtr ClientHandle
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

		public ProductUserId UserId
		{
			set
			{
			}
		}

		public void Set(ref RegisterClientOptions other)
		{
		}

		public void Set(ref RegisterClientOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
