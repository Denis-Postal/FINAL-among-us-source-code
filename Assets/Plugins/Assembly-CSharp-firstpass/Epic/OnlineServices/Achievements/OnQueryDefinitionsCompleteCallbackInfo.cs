namespace Epic.OnlineServices.Achievements
{
	public struct OnQueryDefinitionsCompleteCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref OnQueryDefinitionsCompleteCallbackInfoInternal other)
		{
		}
	}
}
