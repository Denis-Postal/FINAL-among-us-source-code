namespace Epic.OnlineServices.Achievements
{
	public struct OnUnlockAchievementsCompleteCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public ProductUserId UserId { get; set; }

		public uint AchievementsCount { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref OnUnlockAchievementsCompleteCallbackInfoInternal other)
		{
		}
	}
}
