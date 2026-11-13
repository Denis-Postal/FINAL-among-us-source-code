using System;
using System.Net;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[Serializable]
	[StructLayout((LayoutKind)0, Pack = 1, Size = 20)]
	public struct SteamIPAddress_t
	{
		private long m_ip0;

		private long m_ip1;

		private ESteamIPType m_eType;

		public SteamIPAddress_t(IPAddress iPAddress)
		{
			m_ip0 = 0L;
			m_ip1 = 0L;
			m_eType = ESteamIPType.k_ESteamIPTypeIPv4;
		}

		public IPAddress ToIPAddress()
		{
			return null;
		}

		public override string ToString()
		{
			return null;
		}

		public ESteamIPType GetIPType()
		{
			return ESteamIPType.k_ESteamIPTypeIPv4;
		}

		public bool IsSet()
		{
			return false;
		}
	}
}
