using System.Runtime.InteropServices;

namespace Steamworks
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	[CallbackIdentity(1030)]
	public struct TimedTrialStatus_t
	{
		public const int k_iCallback = 1030;

		public AppId_t m_unAppID;

		public bool m_bIsOffline;

		public uint m_unSecondsAllowed;

		public uint m_unSecondsPlayed;
	}
}
