using System.Runtime.InteropServices;

namespace Steamworks
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	[CallbackIdentity(5303)]
	public struct ReservationNotificationCallback_t
	{
		public const int k_iCallback = 5303;

		public PartyBeaconID_t m_ulBeaconID;

		public CSteamID m_steamIDJoiner;
	}
}
