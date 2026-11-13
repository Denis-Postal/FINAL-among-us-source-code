using System;

namespace Steamworks
{
	[Serializable]
	public struct ISteamNetworkingConnectionSignaling
	{
		public bool SendSignal(HSteamNetConnection hConn, ref SteamNetConnectionInfo_t info, IntPtr pMsg, int cbMsg)
		{
			return false;
		}

		public void Release()
		{
		}
	}
}
