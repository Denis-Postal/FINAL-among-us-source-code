using System;

namespace Epic.OnlineServices
{
	public sealed class ProductUserId : Handle
	{
		public const int ProductuseridMaxLength = 32;

		private string _id;

		public ProductUserId()
		{
		}

		public ProductUserId(IntPtr innerHandle)
			: base(innerHandle)
		{
		}

		public ProductUserId(string id)
		{
			_id = id;
		}

		public static ProductUserId FromString(Utf8String productUserIdString)
		{
			if (productUserIdString == null)
			{
				return null;
			}
			return new ProductUserId(productUserIdString.ToString());
		}

		public static explicit operator ProductUserId(Utf8String value)
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

		public static explicit operator Utf8String(ProductUserId value)
		{
			if (value == null)
			{
				return null;
			}
			return (Utf8String)value.ToString();
		}
	}
}
