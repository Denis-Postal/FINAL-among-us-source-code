using System;

namespace Steamworks
{
	public static class SteamNetworkingSockets
	{
		public static HSteamListenSocket CreateListenSocketIP(ref SteamNetworkingIPAddr localAddress, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			return default;
		}

		public static HSteamNetConnection ConnectByIPAddress(ref SteamNetworkingIPAddr address, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			return default;
		}

		public static HSteamListenSocket CreateListenSocketP2P(int nLocalVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			return default;
		}

		public static HSteamNetConnection ConnectP2P(ref SteamNetworkingIdentity identityRemote, int nRemoteVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			return default;
		}

		public static EResult AcceptConnection(HSteamNetConnection hConn)
		{
			return EResult.k_EResultNone;
		}

		public static bool CloseConnection(HSteamNetConnection hPeer, int nReason, string pszDebug, bool bEnableLinger)
		{
			return false;
		}

		public static bool CloseListenSocket(HSteamListenSocket hSocket)
		{
			return false;
		}

		public static bool SetConnectionUserData(HSteamNetConnection hPeer, long nUserData)
		{
			return false;
		}

		public static long GetConnectionUserData(HSteamNetConnection hPeer)
		{
			return 0L;
		}

		public static void SetConnectionName(HSteamNetConnection hPeer, string pszName)
		{
		}

		public static bool GetConnectionName(HSteamNetConnection hPeer, out string pszName, int nMaxLen)
		{
			pszName = null;
			return false;
		}

		public static EResult SendMessageToConnection(HSteamNetConnection hConn, IntPtr pData, uint cbData, int nSendFlags, out long pOutMessageNumber)
		{
			pOutMessageNumber = default;
			return EResult.k_EResultNone;
		}

		public static void SendMessages(int nMessages, SteamNetworkingMessage_t[] pMessages, long[] pOutMessageNumberOrResult)
		{
		}

		public static EResult FlushMessagesOnConnection(HSteamNetConnection hConn)
		{
			return EResult.k_EResultNone;
		}

		public static int ReceiveMessagesOnConnection(HSteamNetConnection hConn, IntPtr[] ppOutMessages, int nMaxMessages)
		{
			return 0;
		}

		public static bool GetConnectionInfo(HSteamNetConnection hConn, out SteamNetConnectionInfo_t pInfo)
		{
			pInfo = default;
			return false;
		}

		public static bool GetQuickConnectionStatus(HSteamNetConnection hConn, out SteamNetworkingQuickConnectionStatus pStats)
		{
			pStats = default;
			return false;
		}

		public static int GetDetailedConnectionStatus(HSteamNetConnection hConn, out string pszBuf, int cbBuf)
		{
			pszBuf = null;
			return 0;
		}

		public static bool GetListenSocketAddress(HSteamListenSocket hSocket, out SteamNetworkingIPAddr address)
		{
			address = default;
			return false;
		}

		public static bool CreateSocketPair(out HSteamNetConnection pOutConnection1, out HSteamNetConnection pOutConnection2, bool bUseNetworkLoopback, ref SteamNetworkingIdentity pIdentity1, ref SteamNetworkingIdentity pIdentity2)
		{
			pOutConnection1 = default;
			pOutConnection2 = default;
			return false;
		}

		public static bool GetIdentity(out SteamNetworkingIdentity pIdentity)
		{
			pIdentity = default;
			return false;
		}

		public static ESteamNetworkingAvailability InitAuthentication()
		{
			return ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Unknown;
		}

		public static ESteamNetworkingAvailability GetAuthenticationStatus(out SteamNetAuthenticationStatus_t pDetails)
		{
			pDetails = default;
			return ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Unknown;
		}

		public static HSteamNetPollGroup CreatePollGroup()
		{
			return default;
		}

		public static bool DestroyPollGroup(HSteamNetPollGroup hPollGroup)
		{
			return false;
		}

		public static bool SetConnectionPollGroup(HSteamNetConnection hConn, HSteamNetPollGroup hPollGroup)
		{
			return false;
		}

		public static int ReceiveMessagesOnPollGroup(HSteamNetPollGroup hPollGroup, IntPtr[] ppOutMessages, int nMaxMessages)
		{
			return 0;
		}

		public static bool ReceivedRelayAuthTicket(IntPtr pvTicket, int cbTicket, out SteamDatagramRelayAuthTicket pOutParsedTicket)
		{
			pOutParsedTicket = default;
			return false;
		}

		public static int FindRelayAuthTicketForServer(ref SteamNetworkingIdentity identityGameServer, int nRemoteVirtualPort, out SteamDatagramRelayAuthTicket pOutParsedTicket)
		{
			pOutParsedTicket = default;
			return 0;
		}

		public static HSteamNetConnection ConnectToHostedDedicatedServer(ref SteamNetworkingIdentity identityTarget, int nRemoteVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			return default;
		}

		public static ushort GetHostedDedicatedServerPort()
		{
			return 0;
		}

		public static SteamNetworkingPOPID GetHostedDedicatedServerPOPID()
		{
			return default;
		}

		public static EResult GetHostedDedicatedServerAddress(out SteamDatagramHostedAddress pRouting)
		{
			pRouting = default;
			return EResult.k_EResultNone;
		}

		public static HSteamListenSocket CreateHostedDedicatedServerListenSocket(int nLocalVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			return default;
		}

		public static EResult GetGameCoordinatorServerLogin(out SteamDatagramGameCoordinatorServerLogin pLoginInfo, out int pcbSignedBlob, IntPtr pBlob)
		{
			pLoginInfo = default;
			pcbSignedBlob = default;
			return EResult.k_EResultNone;
		}

		public static HSteamNetConnection ConnectP2PCustomSignaling(out ISteamNetworkingConnectionSignaling pSignaling, ref SteamNetworkingIdentity pPeerIdentity, int nRemoteVirtualPort, int nOptions, SteamNetworkingConfigValue_t[] pOptions)
		{
			pSignaling = default;
			return default;
		}

		public static bool ReceivedP2PCustomSignal(IntPtr pMsg, int cbMsg, out ISteamNetworkingSignalingRecvContext pContext)
		{
			pContext = default;
			return false;
		}

		public static bool GetCertificateRequest(out int pcbBlob, IntPtr pBlob, out SteamNetworkingErrMsg errMsg)
		{
			pcbBlob = default;
			errMsg = default;
			return false;
		}

		public static bool SetCertificate(IntPtr pCertificate, int cbCertificate, out SteamNetworkingErrMsg errMsg)
		{
			errMsg = default;
			return false;
		}

		public static void RunCallbacks()
		{
		}
	}
}
