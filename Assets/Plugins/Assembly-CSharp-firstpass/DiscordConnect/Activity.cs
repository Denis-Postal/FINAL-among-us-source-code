using System.Collections.Generic;

namespace DiscordConnect
{
	public struct Activity
	{
		public int Type;

		public long ApplicationId;

		public string SessionId;

		public string Name;

		public string Platform;

		private string SupportedPlatformsString;

		public string State;

		public string Details;

		public ActivityTimestamps Timestamps;

		public ActivityAssets Assets;

		public ActivityParty Party;

		public ActivitySecrets Secrets;

		public List<string> SupportedPlatforms
		{
			get
			{
				return null;
			}
			set
			{
			}
		}
	}
}
