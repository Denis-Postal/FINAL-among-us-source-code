namespace Epic.OnlineServices.ProgressionSnapshot
{
	public struct DeleteSnapshotCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref DeleteSnapshotCallbackInfoInternal other)
		{
		}
	}
}
