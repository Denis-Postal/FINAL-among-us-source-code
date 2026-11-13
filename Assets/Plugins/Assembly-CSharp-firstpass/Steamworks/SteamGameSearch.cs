namespace Steamworks
{
	public static class SteamGameSearch
	{
		public static EGameSearchErrorCode_t AddGameSearchParams(string pchKeyToFind, string pchValuesToFind)
		{
			return (EGameSearchErrorCode_t)0;
		}

		public static EGameSearchErrorCode_t SearchForGameWithLobby(CSteamID steamIDLobby, int nPlayerMin, int nPlayerMax)
		{
			return (EGameSearchErrorCode_t)0;
		}

		public static EGameSearchErrorCode_t SearchForGameSolo(int nPlayerMin, int nPlayerMax)
		{
			return (EGameSearchErrorCode_t)0;
		}

		public static EGameSearchErrorCode_t AcceptGame()
		{
			return (EGameSearchErrorCode_t)0;
		}

		public static EGameSearchErrorCode_t DeclineGame()
		{
			return (EGameSearchErrorCode_t)0;
		}

		public static EGameSearchErrorCode_t RetrieveConnectionDetails(CSteamID steamIDHost, out string pchConnectionDetails, int cubConnectionDetails)
		{
			pchConnectionDetails = null;
			return (EGameSearchErrorCode_t)0;
		}

		public static EGameSearchErrorCode_t EndGameSearch()
		{
			return (EGameSearchErrorCode_t)0;
		}

		public static EGameSearchErrorCode_t SetGameHostParams(string pchKey, string pchValue)
		{
			return (EGameSearchErrorCode_t)0;
		}

		public static EGameSearchErrorCode_t SetConnectionDetails(string pchConnectionDetails, int cubConnectionDetails)
		{
			return (EGameSearchErrorCode_t)0;
		}

		public static EGameSearchErrorCode_t RequestPlayersForGame(int nPlayerMin, int nPlayerMax, int nMaxTeamSize)
		{
			return (EGameSearchErrorCode_t)0;
		}

		public static EGameSearchErrorCode_t HostConfirmGameStart(ulong ullUniqueGameID)
		{
			return (EGameSearchErrorCode_t)0;
		}

		public static EGameSearchErrorCode_t CancelRequestPlayersForGame()
		{
			return (EGameSearchErrorCode_t)0;
		}

		public static EGameSearchErrorCode_t SubmitPlayerResult(ulong ullUniqueGameID, CSteamID steamIDPlayer, EPlayerResult_t EPlayerResult)
		{
			return (EGameSearchErrorCode_t)0;
		}

		public static EGameSearchErrorCode_t EndGame(ulong ullUniqueGameID)
		{
			return (EGameSearchErrorCode_t)0;
		}
	}
}
