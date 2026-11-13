using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LobbyDetailsCopyMemberAttributeByIndexOptionsInternal : ISettable<LobbyDetailsCopyMemberAttributeByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_TargetUserId;

		private uint m_AttrIndex;

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public uint AttrIndex
		{
			set
			{
			}
		}

		public void Set(ref LobbyDetailsCopyMemberAttributeByIndexOptions other)
		{
		}

		public void Set(ref LobbyDetailsCopyMemberAttributeByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
