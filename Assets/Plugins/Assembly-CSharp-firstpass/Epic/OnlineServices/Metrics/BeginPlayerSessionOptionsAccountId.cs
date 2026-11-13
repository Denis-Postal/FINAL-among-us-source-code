namespace Epic.OnlineServices.Metrics
{
	public struct BeginPlayerSessionOptionsAccountId
	{
		private MetricsAccountIdType m_AccountIdType;

		private EpicAccountId m_Epic;

		private Utf8String m_External;

		public MetricsAccountIdType AccountIdType
		{
			get
			{
				return MetricsAccountIdType.Epic;
			}
			private set
			{
			}
		}

		public EpicAccountId Epic
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String External
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public static implicit operator BeginPlayerSessionOptionsAccountId(EpicAccountId value)
		{
			return default;
		}

		public static implicit operator BeginPlayerSessionOptionsAccountId(Utf8String value)
		{
			return default;
		}

		public static implicit operator BeginPlayerSessionOptionsAccountId(string value)
		{
			return default;
		}

		internal void Set(ref BeginPlayerSessionOptionsAccountIdInternal other)
		{
		}
	}
}
