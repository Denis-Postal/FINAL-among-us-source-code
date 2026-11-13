using System.ComponentModel;

namespace DiscordConnect
{
	public enum OAuth2Scopes
	{
		[Description("identify")]
		Identify = 0,
		[Description("relationships.read")]
		RelationshipsRead = 1,
		[Description("activities.write")]
		ActivitiesWrite = 2
	}
}
