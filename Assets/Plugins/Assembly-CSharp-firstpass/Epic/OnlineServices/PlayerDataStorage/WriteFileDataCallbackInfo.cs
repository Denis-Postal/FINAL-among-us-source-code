namespace Epic.OnlineServices.PlayerDataStorage
{
	public struct WriteFileDataCallbackInfo : ICallbackInfo
	{
		public object ClientData { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public Utf8String Filename { get; set; }

		public uint DataBufferLengthBytes { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref WriteFileDataCallbackInfoInternal other)
		{
		}
	}
}
