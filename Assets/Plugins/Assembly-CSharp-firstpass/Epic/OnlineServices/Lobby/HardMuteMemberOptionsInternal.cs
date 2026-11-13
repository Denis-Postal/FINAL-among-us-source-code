using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct HardMuteMemberOptionsInternal : ISettable<HardMuteMemberOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LobbyId;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

		private int m_HardMute;

		public Utf8String LobbyId
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

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public bool HardMute
		{
			set
			{
			}
		}

		public void Set(ref HardMuteMemberOptions other)
		{
		}

		public void Set(ref HardMuteMemberOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
