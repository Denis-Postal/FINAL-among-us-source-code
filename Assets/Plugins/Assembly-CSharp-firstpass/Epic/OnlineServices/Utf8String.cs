using System.Diagnostics;

namespace Epic.OnlineServices
{
	[DebuggerDisplay("{ToString()}")]
	public sealed class Utf8String
	{
		public static Utf8String EmptyString;

		public int Length { get; private set; }

		public byte[] Bytes { get; private set; }

		private string Utf16
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public byte this[int index]
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public Utf8String()
		{
		}

		public Utf8String(byte[] bytes)
		{
		}

		public Utf8String(string value)
		{
		}

		public static explicit operator Utf8String(byte[] bytes)
		{
			return null;
		}

		public static explicit operator byte[](Utf8String u8str)
		{
			return null;
		}

		public static implicit operator Utf8String(string str)
		{
			return null;
		}

		public static implicit operator string(Utf8String u8str)
		{
			return null;
		}

		public static Utf8String operator +(Utf8String left, Utf8String right)
		{
			return null;
		}

		public static bool operator ==(Utf8String left, Utf8String right)
		{
			return false;
		}

		public static bool operator !=(Utf8String left, Utf8String right)
		{
			return false;
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public override string ToString()
		{
			return null;
		}

		public override int GetHashCode()
		{
			return 0;
		}
	}
}
