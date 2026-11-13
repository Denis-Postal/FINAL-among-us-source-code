namespace Epic.OnlineServices.Reports
{
	public struct SendPlayerBehaviorReportCompleteCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref SendPlayerBehaviorReportCompleteCallbackInfoInternal other)
		{
		}
	}
}
