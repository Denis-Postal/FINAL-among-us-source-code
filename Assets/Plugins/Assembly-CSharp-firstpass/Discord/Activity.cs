namespace Discord
{
	public struct Activity
	{
		public ActivityType Type;

		public long ApplicationId;

		public string Name;

		public string State;

		public string Details;

		public ActivityTimestamps Timestamps;

		public ActivityAssets Assets;

		public ActivityParty Party;

		public ActivitySecrets Secrets;

		public bool Instance;

		public uint SupportedPlatforms;
	}
}
