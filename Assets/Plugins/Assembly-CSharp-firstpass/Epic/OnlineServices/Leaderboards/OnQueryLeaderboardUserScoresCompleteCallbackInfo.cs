namespace Epic.OnlineServices.Leaderboards
{
	public struct OnQueryLeaderboardUserScoresCompleteCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref OnQueryLeaderboardUserScoresCompleteCallbackInfoInternal other)
		{
		}
	}
}
