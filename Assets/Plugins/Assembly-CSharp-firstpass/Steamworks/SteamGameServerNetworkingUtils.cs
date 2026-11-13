using System;

namespace Steamworks
{
	public static class SteamGameServerNetworkingUtils
	{
		public static IntPtr AllocateMessage(int cbAllocateBuffer)
		{
			return (IntPtr)0;
		}

		public static void InitRelayNetworkAccess()
		{
		}

		public static ESteamNetworkingAvailability GetRelayNetworkStatus(out SteamRelayNetworkStatus_t pDetails)
		{
			pDetails = default;
			return ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Unknown;
		}

		public static float GetLocalPingLocation(out SteamNetworkPingLocation_t result)
		{
			result = default;
			return 0f;
		}

		public static int EstimatePingTimeBetweenTwoLocations(ref SteamNetworkPingLocation_t location1, ref SteamNetworkPingLocation_t location2)
		{
			return 0;
		}

		public static int EstimatePingTimeFromLocalHost(ref SteamNetworkPingLocation_t remoteLocation)
		{
			return 0;
		}

		public static void ConvertPingLocationToString(ref SteamNetworkPingLocation_t location, out string pszBuf, int cchBufSize)
		{
			pszBuf = null;
		}

		public static bool ParsePingLocationString(string pszString, out SteamNetworkPingLocation_t result)
		{
			result = default;
			return false;
		}

		public static bool CheckPingDataUpToDate(float flMaxAgeSeconds)
		{
			return false;
		}

		public static int GetPingToDataCenter(SteamNetworkingPOPID popID, out SteamNetworkingPOPID pViaRelayPoP)
		{
			pViaRelayPoP = default;
			return 0;
		}

		public static int GetDirectPingToPOP(SteamNetworkingPOPID popID)
		{
			return 0;
		}

		public static int GetPOPCount()
		{
			return 0;
		}

		public static int GetPOPList(out SteamNetworkingPOPID list, int nListSz)
		{
			list = default;
			return 0;
		}

		public static SteamNetworkingMicroseconds GetLocalTimestamp()
		{
			return default;
		}

		public static void SetDebugOutputFunction(ESteamNetworkingSocketsDebugOutputType eDetailLevel, FSteamNetworkingSocketsDebugOutput pfnFunc)
		{
		}

		public static bool SetConfigValue(ESteamNetworkingConfigValue eValue, ESteamNetworkingConfigScope eScopeType, IntPtr scopeObj, ESteamNetworkingConfigDataType eDataType, IntPtr pArg)
		{
			return false;
		}

		public static ESteamNetworkingGetConfigValueResult GetConfigValue(ESteamNetworkingConfigValue eValue, ESteamNetworkingConfigScope eScopeType, IntPtr scopeObj, out ESteamNetworkingConfigDataType pOutDataType, IntPtr pResult, out ulong cbResult)
		{
			pOutDataType = default;
			cbResult = default;
			return (ESteamNetworkingGetConfigValueResult)0;
		}

		public static bool GetConfigValueInfo(ESteamNetworkingConfigValue eValue, IntPtr pOutName, out ESteamNetworkingConfigDataType pOutDataType, out ESteamNetworkingConfigScope pOutScope, out ESteamNetworkingConfigValue pOutNextValue)
		{
			pOutDataType = default;
			pOutScope = default;
			pOutNextValue = default;
			return false;
		}

		public static ESteamNetworkingConfigValue GetFirstConfigValue()
		{
			return ESteamNetworkingConfigValue.k_ESteamNetworkingConfig_Invalid;
		}

		public static void SteamNetworkingIPAddr_ToString(ref SteamNetworkingIPAddr addr, out string buf, uint cbBuf, bool bWithPort)
		{
			buf = null;
		}

		public static bool SteamNetworkingIPAddr_ParseString(out SteamNetworkingIPAddr pAddr, string pszStr)
		{
			pAddr = default;
			return false;
		}

		public static void SteamNetworkingIdentity_ToString(ref SteamNetworkingIdentity identity, out string buf, uint cbBuf)
		{
			buf = null;
		}

		public static bool SteamNetworkingIdentity_ParseString(out SteamNetworkingIdentity pIdentity, string pszStr)
		{
			pIdentity = default;
			return false;
		}
	}
}
