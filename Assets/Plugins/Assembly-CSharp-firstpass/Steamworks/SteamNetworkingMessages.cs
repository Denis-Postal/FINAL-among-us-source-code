using System;

namespace Steamworks
{
	public static class SteamNetworkingMessages
	{
		public static EResult SendMessageToUser(ref SteamNetworkingIdentity identityRemote, IntPtr pubData, uint cubData, int nSendFlags, int nRemoteChannel)
		{
			return EResult.k_EResultNone;
		}

		public static int ReceiveMessagesOnChannel(int nLocalChannel, IntPtr[] ppOutMessages, int nMaxMessages)
		{
			return 0;
		}

		public static bool AcceptSessionWithUser(ref SteamNetworkingIdentity identityRemote)
		{
			return false;
		}

		public static bool CloseSessionWithUser(ref SteamNetworkingIdentity identityRemote)
		{
			return false;
		}

		public static bool CloseChannelWithUser(ref SteamNetworkingIdentity identityRemote, int nLocalChannel)
		{
			return false;
		}

		public static ESteamNetworkingConnectionState GetSessionConnectionInfo(ref SteamNetworkingIdentity identityRemote, out SteamNetConnectionInfo_t pConnectionInfo, out SteamNetworkingQuickConnectionStatus pQuickStatus)
		{
			pConnectionInfo = default;
			pQuickStatus = default;
			return ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_None;
		}
	}
}
