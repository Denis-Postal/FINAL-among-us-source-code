using System;

namespace Epic.OnlineServices
{
	public abstract class Handle : IEquatable<Handle>, IFormattable
	{
		public IntPtr InnerHandle { get; internal set; }

		public Handle()
		{
		}

		public Handle(IntPtr innerHandle)
		{
		}

		public static bool operator ==(Handle left, Handle right)
		{
			return false;
		}

		public static bool operator !=(Handle left, Handle right)
		{
			return false;
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public bool Equals(Handle other)
		{
			return false;
		}

		public override string ToString()
		{
			return null;
		}

		public virtual string ToString(string format, IFormatProvider formatProvider)
		{
			return null;
		}
	}
}
