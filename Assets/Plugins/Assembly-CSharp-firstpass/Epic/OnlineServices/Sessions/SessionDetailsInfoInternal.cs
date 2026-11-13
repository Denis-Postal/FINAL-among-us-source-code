using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 56)]
	internal struct SessionDetailsInfoInternal : IGettable<SessionDetailsInfo>, ISettable<SessionDetailsInfo>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_SessionId;

		private IntPtr m_HostAddress;

		private uint m_NumOpenPublicConnections;

		private IntPtr m_Settings;

		private IntPtr m_OwnerUserId;

		private IntPtr m_OwnerServerClientId;

		public Utf8String SessionId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String HostAddress
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public uint NumOpenPublicConnections
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public SessionDetailsSettings? Settings
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ProductUserId OwnerUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String OwnerServerClientId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref SessionDetailsInfo other)
		{
		}

		public void Set(ref SessionDetailsInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out SessionDetailsInfo output)
		{
			output = default;
		}
	}
}
