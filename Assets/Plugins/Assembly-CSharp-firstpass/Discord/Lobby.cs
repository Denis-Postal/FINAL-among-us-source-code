namespace Discord
{
	public struct Lobby
	{
		public long Id;

		public LobbyType Type;

		public long OwnerId;

		public string Secret;

		public uint Capacity;

		public bool Locked;
	}
}
