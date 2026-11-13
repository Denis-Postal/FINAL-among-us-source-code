namespace Steamworks
{
	public static class SteamRemotePlay
	{
		public static uint GetSessionCount()
		{
			return 0u;
		}

		public static RemotePlaySessionID_t GetSessionID(int iSessionIndex)
		{
			return default;
		}

		public static CSteamID GetSessionSteamID(RemotePlaySessionID_t unSessionID)
		{
			return default;
		}

		public static string GetSessionClientName(RemotePlaySessionID_t unSessionID)
		{
			return null;
		}

		public static ESteamDeviceFormFactor GetSessionClientFormFactor(RemotePlaySessionID_t unSessionID)
		{
			return ESteamDeviceFormFactor.k_ESteamDeviceFormFactorUnknown;
		}

		public static bool BGetSessionClientResolution(RemotePlaySessionID_t unSessionID, out int pnResolutionX, out int pnResolutionY)
		{
			pnResolutionX = default;
			pnResolutionY = default;
			return false;
		}

		public static bool BSendRemotePlayTogetherInvite(CSteamID steamIDFriend)
		{
			return false;
		}
	}
}
