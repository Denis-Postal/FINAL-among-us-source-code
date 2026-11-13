using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LobbyDetailsMemberInfoInternal : IGettable<LobbyDetailsMemberInfo>, ISettable<LobbyDetailsMemberInfo>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_UserId;

		private uint m_Platform;

		private int m_AllowsCrossplay;

		public ProductUserId UserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public uint Platform
		{
			get
			{
				return 0u;
			}
			set
			{
			}
		}

		public bool AllowsCrossplay
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public void Set(ref LobbyDetailsMemberInfo other)
		{
		}

		public void Set(ref LobbyDetailsMemberInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LobbyDetailsMemberInfo output)
		{
			output = default;
		}
	}
}
