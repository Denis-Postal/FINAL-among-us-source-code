namespace Epic.OnlineServices.TitleStorage
{
	public struct FileTransferProgressCallbackInfo : ICallbackInfo
	{
		public object ClientData { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public Utf8String Filename { get; set; }

		public uint BytesTransferred { get; set; }

		public uint TotalFileSizeBytes { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref FileTransferProgressCallbackInfoInternal other)
		{
		}
	}
}
