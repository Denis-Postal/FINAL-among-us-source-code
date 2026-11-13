namespace Epic.OnlineServices.Stats
{
	public struct IngestStatCompleteCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public ProductUserId TargetUserId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref IngestStatCompleteCallbackInfoInternal other)
		{
		}
	}
}
