using System;

namespace Steamworks
{
	[Serializable]
	public struct InputAnalogActionHandle_t : IEquatable<InputAnalogActionHandle_t>, IComparable<InputAnalogActionHandle_t>
	{
		public ulong m_InputAnalogActionHandle;

		public InputAnalogActionHandle_t(ulong value)
		{
			m_InputAnalogActionHandle = 0uL;
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

		public static bool operator ==(InputAnalogActionHandle_t x, InputAnalogActionHandle_t y)
		{
			return false;
		}

		public static bool operator !=(InputAnalogActionHandle_t x, InputAnalogActionHandle_t y)
		{
			return false;
		}

		public static explicit operator InputAnalogActionHandle_t(ulong value)
		{
			return default;
		}

		public static explicit operator ulong(InputAnalogActionHandle_t that)
		{
			return 0uL;
		}

		public bool Equals(InputAnalogActionHandle_t other)
		{
			return false;
		}

		public int CompareTo(InputAnalogActionHandle_t other)
		{
			return 0;
		}
	}
}
