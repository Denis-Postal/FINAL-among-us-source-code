namespace Epic.OnlineServices.ProgressionSnapshot
{
	public struct SubmitSnapshotCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public uint SnapshotId { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref SubmitSnapshotCallbackInfoInternal other)
		{
		}
	}
}
