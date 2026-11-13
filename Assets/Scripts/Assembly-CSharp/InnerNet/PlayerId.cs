namespace InnerNet
{
	public struct PlayerId
	{
		public static readonly PlayerId Invalid = new PlayerId(byte.MaxValue);

		public readonly byte Value;

		public PlayerId(byte value)
		{
			Value = value;
		}

		public static implicit operator byte(PlayerId playerId)
		{
			return playerId.Value;
		}

		public static implicit operator PlayerId(byte value)
		{
			return new PlayerId(value);
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", "PlayerId", Value);
		}
	}
}
