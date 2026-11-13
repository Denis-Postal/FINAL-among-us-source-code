using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct JoinLobbyOptionsInternal : ISettable<JoinLobbyOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LobbyDetailsHandle;

		private IntPtr m_LocalUserId;

		private int m_PresenceEnabled;

		private IntPtr m_LocalRTCOptions;

		private int m_CrossplayOptOut;

		public LobbyDetails LobbyDetailsHandle
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

		public LocalRTCOptions? LocalRTCOptions
		{
			set
			{
			}
		}

		public bool CrossplayOptOut
		{
			set
			{
			}
		}

		public void Set(ref JoinLobbyOptions other)
		{
		}

		public void Set(ref JoinLobbyOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
