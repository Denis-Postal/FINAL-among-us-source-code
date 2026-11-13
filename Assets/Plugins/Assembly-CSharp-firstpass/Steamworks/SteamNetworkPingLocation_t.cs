using System.Runtime.InteropServices;

namespace Steamworks
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 512)]
	public struct SteamNetworkPingLocation_t
	{
		public byte[] m_data;
	}
}
