using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct LobbyDetailsCopyMemberInfoOptionsInternal : ISettable<LobbyDetailsCopyMemberInfoOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_TargetUserId;

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public void Set(ref LobbyDetailsCopyMemberInfoOptions other)
		{
		}

		public void Set(ref LobbyDetailsCopyMemberInfoOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
