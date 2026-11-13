namespace Epic.OnlineServices.Achievements
{
	public struct OnQueryPlayerAchievementsCompleteCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public ProductUserId TargetUserId { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref OnQueryPlayerAchievementsCompleteCallbackInfoInternal other)
		{
		}
	}
}
