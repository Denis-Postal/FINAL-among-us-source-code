using System.Runtime.InteropServices;

namespace Steamworks
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 20)]
	[CallbackIdentity(166)]
	public struct MarketEligibilityResponse_t
	{
		public const int k_iCallback = 166;

		public bool m_bAllowed;

		public EMarketNotAllowedReasonFlags m_eNotAllowedReason;

		public RTime32 m_rtAllowedAtTime;

		public int m_cdaySteamGuardRequiredDays;

		public int m_cdayNewDeviceCooldown;
	}
}
