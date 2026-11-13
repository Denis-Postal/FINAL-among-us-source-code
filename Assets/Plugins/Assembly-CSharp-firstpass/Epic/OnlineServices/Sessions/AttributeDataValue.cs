namespace Epic.OnlineServices.Sessions
{
	public struct AttributeDataValue
	{
		private long? m_AsInt64;

		private double? m_AsDouble;

		private bool? m_AsBool;

		private Utf8String m_AsUtf8;

		private AttributeType m_ValueType;

		public long? AsInt64
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public double? AsDouble
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool? AsBool
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String AsUtf8
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public AttributeType ValueType
		{
			get
			{
				return AttributeType.Boolean;
			}
			private set
			{
			}
		}

		public static implicit operator AttributeDataValue(long value)
		{
			return default;
		}

		public static implicit operator AttributeDataValue(double value)
		{
			return default;
		}

		public static implicit operator AttributeDataValue(bool value)
		{
			return default;
		}

		public static implicit operator AttributeDataValue(Utf8String value)
		{
			return default;
		}

		public static implicit operator AttributeDataValue(string value)
		{
			return default;
		}

		internal void Set(ref AttributeDataValueInternal other)
		{
		}
	}
}
