namespace InnerNet
{
	public struct ClientId
	{
		public const int INVALID_OWNER_ID_VALUE = -1;

		public const int HOST_INHERIT_OWNERSHIP_ID_VALUE = -2;

		public const int SERVER_OWNER_ID_VALUE = -4;

		public static readonly ClientId InvalidOwner = new ClientId(-1);

		public static readonly ClientId HostInheritOwnership = new ClientId(-2);

		public static readonly ClientId ServerOwner = new ClientId(-4);

		public readonly int Value;

		public ClientId(int value)
		{
			Value = value;
		}

		public static implicit operator int(ClientId clientId)
		{
			return clientId.Value;
		}

		public static implicit operator ClientId(int value)
		{
			return new ClientId(value);
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", "ClientId", Value);
		}
	}
}
