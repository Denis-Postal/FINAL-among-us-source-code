using System.Runtime.InteropServices;

namespace Steamworks
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 280)]
	[CallbackIdentity(5301)]
	public struct JoinPartyCallback_t
	{
		public const int k_iCallback = 5301;

		public EResult m_eResult;

		public PartyBeaconID_t m_ulBeaconID;

		public CSteamID m_SteamIDBeaconOwner;

		private byte[] m_rgchConnectString_;

		public string m_rgchConnectString
		{
			get
			{
				return null;
			}
			set
			{
			}
		}
	}
}
