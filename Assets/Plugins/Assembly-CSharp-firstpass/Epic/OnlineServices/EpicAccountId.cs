using System;

namespace Epic.OnlineServices
{
	public sealed class EpicAccountId : Handle
	{
		public const int EpicaccountidMaxLength = 32;

		private string _id;

		public EpicAccountId()
		{
		}

		public EpicAccountId(IntPtr innerHandle)
			: base(innerHandle)
		{
		}

		public EpicAccountId(string id)
		{
			_id = id;
		}

		public static EpicAccountId FromString(Utf8String accountIdString)
		{
			if (accountIdString == null)
			{
				return null;
			}
			return new EpicAccountId(accountIdString.ToString());
		}

		public static explicit operator EpicAccountId(Utf8String value)
		{
			return FromString(value);
		}

		public bool IsValid()
		{
			return !string.IsNullOrEmpty(_id);
		}

		public Result ToString(out Utf8String outBuffer)
		{
			outBuffer = _id ?? "00000000000000000000000000000001";
			return Result.Success;
		}

		public override string ToString()
		{
			return _id ?? "00000000000000000000000000000001";
		}

		public override string ToString(string format, IFormatProvider formatProvider)
		{
			return ToString();
		}

		public static explicit operator Utf8String(EpicAccountId value)
		{
			if (value == null)
			{
				return null;
			}
			return (Utf8String)value.ToString();
		}
	}
}
