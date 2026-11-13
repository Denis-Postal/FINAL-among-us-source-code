using System;

namespace Steamworks
{
	[Serializable]
	public struct RTime32 : IEquatable<RTime32>, IComparable<RTime32>
	{
		public uint m_RTime32;

		public RTime32(uint value)
		{
			m_RTime32 = 0u;
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

		public static bool operator ==(RTime32 x, RTime32 y)
		{
			return false;
		}

		public static bool operator !=(RTime32 x, RTime32 y)
		{
			return false;
		}

		public static explicit operator RTime32(uint value)
		{
			return default;
		}

		public static explicit operator uint(RTime32 that)
		{
			return 0u;
		}

		public bool Equals(RTime32 other)
		{
			return false;
		}

		public int CompareTo(RTime32 other)
		{
			return 0;
		}
	}
}
