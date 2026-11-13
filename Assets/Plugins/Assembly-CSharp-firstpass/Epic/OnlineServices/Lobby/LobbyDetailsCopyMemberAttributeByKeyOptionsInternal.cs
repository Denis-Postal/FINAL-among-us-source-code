using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LobbyDetailsCopyMemberAttributeByKeyOptionsInternal : ISettable<LobbyDetailsCopyMemberAttributeByKeyOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_TargetUserId;

		private IntPtr m_AttrKey;

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public Utf8String AttrKey
		{
			set
			{
			}
		}

		public void Set(ref LobbyDetailsCopyMemberAttributeByKeyOptions other)
		{
		}

		public void Set(ref LobbyDetailsCopyMemberAttributeByKeyOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
