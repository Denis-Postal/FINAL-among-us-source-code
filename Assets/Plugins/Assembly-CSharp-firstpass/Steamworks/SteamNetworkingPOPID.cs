using System;

namespace Steamworks
{
	[Serializable]
	public struct SteamNetworkingPOPID : IEquatable<SteamNetworkingPOPID>, IComparable<SteamNetworkingPOPID>
	{
		public uint m_SteamNetworkingPOPID;

		public SteamNetworkingPOPID(uint value)
		{
			m_SteamNetworkingPOPID = 0u;
		}

		public override string ToString()
		{
			return null;
		}

		public override bool Equals(object other)
		{
			return false;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static bool operator ==(SteamNetworkingPOPID x, SteamNetworkingPOPID y)
		{
			return false;
		}

		public static bool operator !=(SteamNetworkingPOPID x, SteamNetworkingPOPID y)
		{
			return false;
		}

		public static explicit operator SteamNetworkingPOPID(uint value)
		{
			return default;
		}

		public static explicit operator uint(SteamNetworkingPOPID that)
		{
			return 0u;
		}

		public bool Equals(SteamNetworkingPOPID other)
		{
			return false;
		}

		public int CompareTo(SteamNetworkingPOPID other)
		{
			return 0;
		}
	}
}
