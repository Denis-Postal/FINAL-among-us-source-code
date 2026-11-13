namespace Steamworks
{
	public static class SteamParties
	{
		public static uint GetNumActiveBeacons()
		{
			return 0u;
		}

		public static PartyBeaconID_t GetBeaconByIndex(uint unIndex)
		{
			return default;
		}

		public static bool GetBeaconDetails(PartyBeaconID_t ulBeaconID, out CSteamID pSteamIDBeaconOwner, out SteamPartyBeaconLocation_t pLocation, out string pchMetadata, int cchMetadata)
		{
			pSteamIDBeaconOwner = default;
			pLocation = default;
			pchMetadata = null;
			return false;
		}

		public static SteamAPICall_t JoinParty(PartyBeaconID_t ulBeaconID)
		{
			return default;
		}

		public static bool GetNumAvailableBeaconLocations(out uint puNumLocations)
		{
			puNumLocations = default;
			return false;
		}

		public static bool GetAvailableBeaconLocations(SteamPartyBeaconLocation_t[] pLocationList, uint uMaxNumLocations)
		{
			return false;
		}

		public static SteamAPICall_t CreateBeacon(uint unOpenSlots, ref SteamPartyBeaconLocation_t pBeaconLocation, string pchConnectString, string pchMetadata)
		{
			return default;
		}

		public static void OnReservationCompleted(PartyBeaconID_t ulBeacon, CSteamID steamIDUser)
		{
		}

		public static void CancelReservation(PartyBeaconID_t ulBeacon, CSteamID steamIDUser)
		{
		}

		public static SteamAPICall_t ChangeNumOpenSlots(PartyBeaconID_t ulBeacon, uint unOpenSlots)
		{
			return default;
		}

		public static bool DestroyBeacon(PartyBeaconID_t ulBeacon)
		{
			return false;
		}

		public static bool GetBeaconLocationData(SteamPartyBeaconLocation_t BeaconLocation, ESteamPartyBeaconLocationData eData, out string pchDataStringOut, int cchDataStringOut)
		{
			pchDataStringOut = null;
			return false;
		}
	}
}
