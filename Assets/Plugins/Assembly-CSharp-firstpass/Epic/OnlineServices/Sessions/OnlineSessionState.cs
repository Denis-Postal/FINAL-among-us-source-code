namespace Epic.OnlineServices.Sessions
{
	public enum OnlineSessionState
	{
		NoSession = 0,
		Creating = 1,
		Pending = 2,
		Starting = 3,
		InProgress = 4,
		Ending = 5,
		Ended = 6,
		Destroying = 7
	}
}
