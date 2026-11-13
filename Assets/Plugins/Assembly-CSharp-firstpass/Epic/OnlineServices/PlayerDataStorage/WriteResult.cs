namespace Epic.OnlineServices.PlayerDataStorage
{
	public enum WriteResult
	{
		ContinueWriting = 1,
		CompleteRequest = 2,
		FailRequest = 3,
		CancelRequest = 4
	}
}
