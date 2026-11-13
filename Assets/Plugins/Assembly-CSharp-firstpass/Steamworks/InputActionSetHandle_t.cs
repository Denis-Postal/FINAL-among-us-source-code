using System;

namespace Steamworks
{
	[Serializable]
	public struct InputActionSetHandle_t : IEquatable<InputActionSetHandle_t>, IComparable<InputActionSetHandle_t>
	{
		public ulong m_InputActionSetHandle;

		public InputActionSetHandle_t(ulong value)
		{
			m_InputActionSetHandle = 0uL;
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

		public static bool operator ==(InputActionSetHandle_t x, InputActionSetHandle_t y)
		{
			return false;
		}

		public static bool operator !=(InputActionSetHandle_t x, InputActionSetHandle_t y)
		{
			return false;
		}

		public static explicit operator InputActionSetHandle_t(ulong value)
		{
			return default;
		}

		public static explicit operator ulong(InputActionSetHandle_t that)
		{
			return 0uL;
		}

		public bool Equals(InputActionSetHandle_t other)
		{
			return false;
		}

		public int CompareTo(InputActionSetHandle_t other)
		{
			return 0;
		}
	}
}
