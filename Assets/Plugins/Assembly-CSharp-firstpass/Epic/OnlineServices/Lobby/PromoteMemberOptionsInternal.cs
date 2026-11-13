using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct PromoteMemberOptionsInternal : ISettable<PromoteMemberOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LobbyId;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

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

		public void Set(ref PromoteMemberOptions other)
		{
		}

		public void Set(ref PromoteMemberOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
